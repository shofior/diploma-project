using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace diplom
{
	public partial class Form1 : Form
	{
		private Scene3D scene;
		private float rotationX = 0;
		private float rotationY = 0;
		private float scale = 1.0f;
		private Point lastMousePos;
		private bool showWallsInsteadOfSupports = false;
		private Wall leftWall;
		private Wall rightWall;
		private bool showDeflection = false;
		private float currentElasticityModulus = 2.0e5f;
		private bool forceExists = false;
		private bool momentExists = false;

		public Form1()
		{
			InitializeComponent();
			scene = new Scene3D(pictureBox1.Width, pictureBox1.Height);

			forceMagnitude = 500;
			forcePosition = new Point3D(150, 0, -10);
			scene.AddForce(new Force(forcePosition, forceMagnitude, 0));

			// Початкові параметри балки
			beamStart = new Point3D(0, 0, 0);
			beamEnd = new Point3D(300, 0, 0);
			beamWidth = 20;
			beamHeight = 30;

			// Початкова конфігурація (між стінами)
			showWallsInsteadOfSupports = true;
			InitializeScene();

			pictureBox1.Paint += PictureBox1_Paint;
			pictureBox1.MouseDown += PictureBox1_MouseDown;
			pictureBox1.MouseMove += PictureBox1_MouseMove;
			pictureBox1.MouseWheel += PictureBox1_MouseWheel;
		}

		private void InitializeScene()
		{
			bool hadForce = forceExists;
			bool hadMoment = momentExists;

			scene.ClearAll();

			// Додаємо балку
			scene.AddBeam(beamStart, beamEnd, beamWidth, beamHeight);

			if (scene.Beam != null)
			{
				scene.Beam.SetShowDeflection(showDeflection);
				scene.Beam.SetElasticityModulus(currentElasticityModulus);
			}

			if (showWallsInsteadOfSupports)
			{
				// Оновлюємо позиції стін
				leftWall = new Wall(
					new Point3D(beamStart.X, beamStart.Y - 70, beamStart.Z - 40),
					new Point3D(beamStart.X, beamStart.Y + 70, beamStart.Z - 40),
					80);

				rightWall = new Wall(
					new Point3D(beamEnd.X, beamEnd.Y - 70, beamEnd.Z - 40),
					new Point3D(beamEnd.X, beamEnd.Y + 70, beamEnd.Z - 40),
					80);

				scene.AddWall(leftWall.Start, leftWall.End, leftWall.Thickness);
				scene.AddWall(rightWall.Start, rightWall.End, rightWall.Thickness);
			}
			else
			{
				// Додаємо опори
				scene.AddRollerSupport(new Point3D(beamStart.X, beamStart.Y, beamStart.Z));
				scene.AddHingeSupport(new Point3D(beamEnd.X, beamEnd.Y, beamEnd.Z));
			}

			// Відновлюємо сили та моменти, якщо вони були
			if (hadForce)
			{
				scene.AddForce(new Force(forcePosition, forceMagnitude, 0));
				forceExists = true;
			}

			if (hadMoment)
			{
				scene.AddMoment(new Moment(momentPosition, momentAxis, momentValue));
				momentExists = true;
			}

			pictureBox1.Invalidate();
		}

		private Point3D beamStart = new Point3D(0, 0, 0);
		private Point3D beamEnd = new Point3D(300, 0, 0);
		private float beamWidth = 20;
		private float beamHeight = 30;

		private float GetBeamLength()
		{
			return beamEnd.X - beamStart.X;
		}

		private Point3D rollerSupportPosition = new Point3D(0, 0, -15);
		private Point3D hingeSupportPosition = new Point3D(300, 0, -15);

		private Point3D forcePosition = new Point3D(150, 0, 0);
		private Vector3D forceDirection = new Vector3D(0, 0, -1);
		private float forceMagnitude = 500;

		private Point3D momentPosition = new Point3D(150, 0, 0);
		private Vector3D momentAxis = new Vector3D(0, -1, 0);
		private float momentValue = 500;		

		private void PictureBox1_Paint(object sender, PaintEventArgs e)
		{
			scene.Render(e.Graphics, rotationX, rotationY, scale);
		}

		private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			lastMousePos = e.Location;
		}

		private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				rotationY += (e.X - lastMousePos.X) * 0.5f;
				rotationX += (e.Y - lastMousePos.Y) * 0.5f;
				lastMousePos = e.Location;
				pictureBox1.Invalidate();
			}
		}

		private void PictureBox1_MouseWheel(object sender, MouseEventArgs e)
		{
			// Змінюємо масштаб (1.1 для збільшення, 0.9 для зменшення)
			float scaleFactor = e.Delta > 0 ? 1.1f : 0.9f;

			// Обмежуємо масштаб (наприклад, від 0.2 до 5)
			float newScale = scale * scaleFactor;
			if (newScale > 0.2f && newScale < 5.0f)
			{
				scale = newScale;
				pictureBox1.Invalidate();
			}
		}


		private void MainForm_Resize(object sender, EventArgs e)
		{
			if (pictureBox1 != null)
			{
				pictureBox1.Invalidate();
			}
		}
		public class Scene3D
		{
			private int width;
			private int height;
			private List<Wall> walls;
			private Hinge[] hinges = new Hinge[2];

			public Scene3D(int width, int height)
			{
				this.width = width;
				this.height = height;
				this.walls = new List<Wall>();
			}

			public Beam Beam { get; private set; }

			public void AddBeam(Point3D start, Point3D end, float width, float height)
			{
				Beam = new Beam(start, end, width, height, forces, moments);
			}

			public void AddWall(Point3D start, Point3D end, float thickness)
			{				
				walls.Add(new Wall(start, end, thickness));
			}

			public void AddHinge(Point3D position, float radius)
			{
				if (hinges[0] == null)
					hinges[0] = new Hinge(position, radius);
				else
					hinges[1] = new Hinge(position, radius);
			}

			private List<Force> forces = new List<Force>();
			private List<Moment> moments = new List<Moment>();			
			private List<object> supports = new List<object>();
			public int ForcesCount => forces.Count;

			public void AddForce(Force force)
			{
				ClearForces();
				forces.Add(force);
			}

			public void AddMoment(Moment moment)
			{
				ClearMoments();
				moments.Add(moment);
			}			

			public void AddRollerSupport(Point3D position)
			{
				supports.Add(new RollerSupport(position));
			}

			public void AddHingeSupport(Point3D position)
			{
				supports.Add(new HingeSupport(position));
			}
			public void ClearForces()
			{
				forces.Clear();
			}

			public void ClearMoments()
			{
				moments.Clear();
			}			

			public void ClearAll()
			{
				Beam = null;
				walls.Clear();
				hinges = new Hinge[2];
				forces.Clear();
				moments.Clear();				
				supports.Clear();
			}

			public void Render(Graphics g, float rotationX, float rotationY, float scale = 1.0f)
			{
				g.Clear(Color.White);
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.InterpolationMode = InterpolationMode.HighQualityBicubic;
				PointF center = new PointF(width / 2, height / 2);

				foreach (var wall in walls)
				{
					wall?.Render(g, center, rotationX, rotationY, scale);
				}

				// Потім інші елементи
				foreach (var support in supports)
				{
					if (support is RollerSupport roller)
						roller.Render(g, center, rotationX, rotationY, scale);
					else if (support is HingeSupport hinge)
						hinge.Render(g, center, rotationX, rotationY, scale);
				}

				Beam.Render(g, center, rotationX, rotationY, scale);			

				foreach (var force in forces)
				{
					force.Render(g, center, rotationX, rotationY, scale);
				}

				foreach (var moment in moments)
				{
					moment.Render(g, center, rotationX, rotationY, scale);
				}
			}
		}

		public class Point3D
		{
			public float X { get; set; }
			public float Y { get; set; }
			public float Z { get; set; }

			public Point3D(float x, float y, float z)
			{
				X = x;
				Y = y;
				Z = z;
			}

			// Додаємо оператор додавання Point3D і Vector3D
			public static Point3D operator +(Point3D p, Vector3D v)
			{
				return new Point3D(p.X + v.X, p.Y + v.Y, p.Z + v.Z);
			}


			public PointF Project(PointF center, float rotationX, float rotationY, float scale = 1.0f)
			{
				// Масштабування
				float sx = X * scale;
				float sy = Y * scale;
				float sz = Z * scale;

				// Обертання навколо осей
				float cosX = (float)Math.Cos(rotationX * Math.PI / 180);
				float sinX = (float)Math.Sin(rotationX * Math.PI / 180);
				float cosY = (float)Math.Cos(rotationY * Math.PI / 180);
				float sinY = (float)Math.Sin(rotationY * Math.PI / 180);

				// Обертання навколо Y (горизонталь)
				float x1 = sx * cosY - sz * sinY;
				float z1 = sx * sinY + sz * cosY;

				// Обертання навколо X (вертикаль)
				float y1 = sy * cosX - z1 * sinX;
				float z2 = sy * sinX + z1 * cosX;

				// Перспектива
				float fov = 1000f;
				float factor = fov / (fov + z2);

				return new PointF(
					center.X + x1 * factor,
					center.Y + y1 * factor);
			}
		}


		public class Beam
		{
			private Point3D start;
			private Point3D end;
			private float width;
			private float height;
			
			private bool showDeflection = false;
			private float elasticityModulus = 2.0e5f; // Модуль пружності (N/mm² для сталі)
			private List<Force> forces;
			private List<Moment> moments;			

			public bool ToggleDeflection()
			{
				showDeflection = !showDeflection;
				return showDeflection;
			}

			public void SetShowDeflection(bool show)
			{
				showDeflection = show;
			}

			public void SetElasticityModulus(float value)
			{
				elasticityModulus = value;
			}

			public float CalculateIy()
			{
				return height * width * width * width / 12f; // Момент інерції для вигину в Y-площині
			}

			public Beam(Point3D start, Point3D end, float width, float height,
					   List<Force> forces, List<Moment> moments)
			{
				this.start = start;
				this.end = end;
				this.width = width;
				this.height = height;
				this.forces = forces;
				this.moments = moments;				
			}


			public void Render(Graphics g, PointF center, float rotationX, float rotationY, float scale = 1.0f)
			{

				if (forces == null || moments == null)
				{
					// Якщо списки навантажень не ініціалізовані, просто показуємо балку
					RenderOriginalBeam(g, center, rotationX, rotationY, scale, 255);
					return;
				}

				if (showDeflection)
				{
					RenderDeflectedBeam(g, center, rotationX, rotationY, scale);
					RenderOriginalBeam(g, center, rotationX, rotationY, scale, 50);
				}
				else
				{
					RenderOriginalBeam(g, center, rotationX, rotationY, scale, 255);
				}
				
			}

			private void RenderDeflectedBeam(Graphics g, PointF center, float rotationX, float rotationY, float scale)
			{
				float L = end.X - start.X;
				float Iy = height * width * width * width / 12f; // Для вигину в XZ
				float Iz = width * height * height * height / 12f; // Для вигину в XY
				float visualScale = 50f; // Коефіцієнт для наочності

				Form1 mainForm = (Form1)Application.OpenForms["Form1"];
				bool isFixed = mainForm.showWallsInsteadOfSupports;
				int segments = 100;
				PointF[] deflectedTop = new PointF[segments + 1];
				PointF[] deflectedBottom = new PointF[segments + 1];

				for (int i = 0; i <= segments; i++)
				{
					float x = i * L / segments;
					Vector3D deflection = BeamDeflectionCalculator.CalculateDeflection3D(
		x, L, elasticityModulus, Iy, Iz, forces, moments, isFixed);

					// Зміщуємо точки з урахуванням прогину в Y та Z
					Point3D topPoint = new Point3D(
						start.X + x,
						start.Y + width - deflection.Y * visualScale,
						start.Z + height / 2 - deflection.Z * visualScale
					);

					Point3D bottomPoint = new Point3D(
						start.X + x,
						start.Y + width - deflection.Y * visualScale,
						start.Z - height / 2 - deflection.Z * visualScale
					);

					deflectedTop[i] = topPoint.Project(center, rotationX, rotationY, scale);
					deflectedBottom[i] = bottomPoint.Project(center, rotationX, rotationY, scale);
				}
				PointF[] deflectedPoints = new PointF[segments + 1];


				// Малюємо деформовану балку
				using (Pen pen = new Pen(Color.FromArgb(200, Color.Red), 2))
				{
					g.DrawCurve(pen, deflectedTop, 0.5f);
					g.DrawCurve(pen, deflectedBottom, 0.5f);

					for (int i = 0; i < segments; i += 5)
						g.DrawLine(pen, deflectedTop[i], deflectedBottom[i]);

				}

			}

			private void RenderOriginalBeam(Graphics g, PointF center, float rotationX, float rotationY, float scale, int alpha)
			{
				// Верхняя грань балки
				Point3D[] topFace = new Point3D[4];
				topFace[0] = new Point3D(start.X, start.Y, start.Z + height / 2);
				topFace[1] = new Point3D(end.X, end.Y, end.Z + height / 2);
				topFace[2] = new Point3D(end.X, end.Y + width, end.Z + height / 2);
				topFace[3] = new Point3D(start.X, start.Y + width, start.Z + height / 2);

				// Нижняя грань балки
				Point3D[] bottomFace = new Point3D[4];
				bottomFace[0] = new Point3D(start.X, start.Y, start.Z - height / 2);
				bottomFace[1] = new Point3D(end.X, end.Y, end.Z - height / 2);
				bottomFace[2] = new Point3D(end.X, end.Y + width, end.Z - height / 2);
				bottomFace[3] = new Point3D(start.X, start.Y + width, start.Z - height / 2);

				// Проецируем точки з урахуванням масштабу
				PointF[] projectedTop = new PointF[4];
				PointF[] projectedBottom = new PointF[4];

				for (int i = 0; i < 4; i++)
				{
					projectedTop[i] = topFace[i].Project(center, rotationX, rotationY, scale); // Додано scale
					projectedBottom[i] = bottomFace[i].Project(center, rotationX, rotationY, scale); // Додано scale
				}

				// Рисуем грани
				using (SolidBrush brush = new SolidBrush(Color.FromArgb(150, Color.SteelBlue)))
				{
					// Верхняя грань
					g.FillPolygon(brush, projectedTop);

					// Нижняя грань
					g.FillPolygon(brush, projectedBottom);

					// Боковые грани
					for (int i = 0; i < 4; i++)
					{
						PointF[] side = new PointF[4];
						side[0] = projectedTop[i];
						side[1] = projectedTop[(i + 1) % 4];
						side[2] = projectedBottom[(i + 1) % 4];
						side[3] = projectedBottom[i];
						g.FillPolygon(brush, side);
					}
				}

				// Контуры
				using (Pen pen = new Pen(Color.DarkBlue, 2))
				{
					g.DrawPolygon(pen, projectedTop);
					g.DrawPolygon(pen, projectedBottom);

					for (int i = 0; i < 4; i++)
					{
						g.DrawLine(pen, projectedTop[i], projectedBottom[i]);
					}
				}
			}
		}

		public class Wall
		{			
			public Point3D Start { get; set; }
			public Point3D End { get; set; }
			public float Thickness { get; set; }

			public Wall(Point3D start, Point3D end, float thickness)
			{
				Start = start;
				End = end;
				Thickness = thickness;
			}

			public void Render(Graphics g, PointF center, float rotationX, float rotationY, float scale)
			{
				// Створюємо 4 точки для стіни (3D)
				Point3D[] wallPoints = new Point3D[4];
				wallPoints[0] = Start;
				wallPoints[1] = End;
				wallPoints[2] = new Point3D(End.X, End.Y, End.Z + Thickness);
				wallPoints[3] = new Point3D(Start.X, Start.Y, Start.Z + Thickness);

				// Проєктуємо точки в 2D
				PointF[] projected = new PointF[4];
				for (int i = 0; i < 4; i++)
				{
					projected[i] = wallPoints[i].Project(center, rotationX, rotationY, scale);
				}

				// Малюємо стіну
				using (SolidBrush brush = new SolidBrush(Color.FromArgb(180, Color.Gray)))
				{
					g.FillPolygon(brush, projected);
				}

				using (Pen pen = new Pen(Color.DarkGray, 2 * scale))
				{
					g.DrawPolygon(pen, projected);
				}
			}
		}

		public class Hinge
		{
			private Point3D position;
			private float radius;

			public Hinge(Point3D position, float radius)
			{
				this.position = position;
				this.radius = radius;
			}

			public void Render(Graphics g, PointF center, float rotationX, float rotationY, float scale = 1.0f)
			{
				PointF center2D = position.Project(center, rotationX, rotationY, scale);
				float scaledRadius = radius * scale; // Використовуємо переданий scale

				// Рисуем шарнир
				using (SolidBrush brush = new SolidBrush(Color.FromArgb(200, Color.Gold)))
				{
					g.FillEllipse(brush, center2D.X - scaledRadius, center2D.Y - scaledRadius,
								 scaledRadius * 2, scaledRadius * 2);
				}

				using (Pen pen = new Pen(Color.DarkGoldenrod, 2))
				{
					g.DrawEllipse(pen, center2D.X - scaledRadius, center2D.Y - scaledRadius,
								 scaledRadius * 2, scaledRadius * 2);

					// Крестовина шарнира
					g.DrawLine(pen, center2D.X - scaledRadius, center2D.Y,
							  center2D.X + scaledRadius, center2D.Y);
					g.DrawLine(pen, center2D.X, center2D.Y - scaledRadius,
							  center2D.X, center2D.Y + scaledRadius);
				}
			}
		}

		public class Force
		{
			public Point3D ApplicationPoint { get; set; }
			public Vector3D Direction { get; set; }
			public float Magnitude { get; set; }
			public Color Color { get; set; } = Color.Red;
			public float AngleDegrees { get; set; } = 0;

			public Force(Point3D applicationPoint, float magnitude, float angleDegrees = 0)
			{
				ApplicationPoint = applicationPoint;
				Magnitude = magnitude;
				AngleDegrees = angleDegrees;
				UpdateDirection();
			}

			private void UpdateDirection()
			{
				float angleRad = AngleDegrees * (float)Math.PI / 180f;
				// Основна складова по Y, можливий нахил по Z
				Direction = new Vector3D(
					0,
					-(float)Math.Cos(angleRad),
					(float)Math.Sin(angleRad));
			}

			public void SetAngle(float degrees)
			{
				AngleDegrees = degrees;
				UpdateDirection();
			}

			public void Render(Graphics g, PointF center, float rotationX, float rotationY, float scale = 1.0f)
			{
				// Точка прикладання сили (зсуваємо трохи вище балки)
				Point3D visualPoint = new Point3D(
					ApplicationPoint.X,
					ApplicationPoint.Y,
					ApplicationPoint.Z + 10); // 10 одиниць вище балки

				PointF start = visualPoint.Project(center, rotationX, rotationY, scale);

				// Кінець стрілки (в напрямку дії сили)
				Vector3D scaledDirection = Direction * (Magnitude * 0.1f * scale);
				Point3D endPoint = visualPoint + scaledDirection;
				PointF end = endPoint.Project(center, rotationX, rotationY, scale);

				// Малюємо стрілку від точки прикладання до кінця
				using (Pen pen = new Pen(Color, 2))
				{
					g.DrawLine(pen, start, end);
					DrawArrow(g, pen, end, start); // Стрілка вказує на балку
				}

				// Підпис сили
				using (Font font = new Font("Arial", 8))
				using (Brush brush = new SolidBrush(Color))
				{
					string forceText = $"{Magnitude} N";
					if (AngleDegrees != 0) forceText += $", {AngleDegrees}°";
					g.DrawString(forceText, font, brush, end.X + 5, end.Y - 10);
				}
			}

			private void DrawArrow(Graphics g, Pen pen, PointF from, PointF to)
			{
				float arrowSize = 8;
				float angle = (float)Math.Atan2(to.Y - from.Y, to.X - from.X);

				
				PointF arrowPoint1 = new PointF(
					to.X - arrowSize * (float)Math.Cos(angle - Math.PI / 6),
					to.Y - arrowSize * (float)Math.Sin(angle - Math.PI / 6));

				PointF arrowPoint2 = new PointF(
					to.X - arrowSize * (float)Math.Cos(angle + Math.PI / 6),
					to.Y - arrowSize * (float)Math.Sin(angle + Math.PI / 6));

				g.DrawLine(pen, to, arrowPoint1);
				g.DrawLine(pen, to, arrowPoint2);
			}
		}


		public class Moment
		{			
			public Point3D ApplicationPoint { get; set; }
			public Vector3D Axis { get; set; }
			public float Magnitude { get; set; }
			public Color Color { get; set; } = Color.Blue;
			public float ArcRadius { get; set; } = 30f; // Радіус дуги моменту

			public Moment(Point3D applicationPoint, Vector3D axis, float magnitude)
			{
				ApplicationPoint = applicationPoint;
				//Axis = new Vector3D( 1, 0 ,0);
				Axis = axis;
				Magnitude = magnitude;
			}
			
			public void Render(Graphics g, PointF center, float rotationX, float rotationY, float scale = 1.0f)
			{
				// 1. Визначаємо реальні 3D координати для візуалізації моменту
				Point3D center3D = ApplicationPoint;
				float radius = ArcRadius;

				// 2. Створюємо точки для дуги в 3D просторі
				List<Point3D> arcPoints = new List<Point3D>();
				int segments = 12;
				for (int i = 0; i <= segments; i++)
				{
					float angle = (float)i / segments * 270f * (Magnitude > 0 ? 1 : -1);
					Point3D point = CalculateArcPoint3D(center3D, angle);
					arcPoints.Add(point);
				}

				// 3. Проєктуємо точки дуги на 2D з урахуванням обертання камери
				PointF[] projectedArc = arcPoints.Select(p => p.Project(center, rotationX, rotationY, scale)).ToArray();

				// 4. Малюємо дугу
				using (Pen pen = new Pen(Color, 2 * scale))
				{
					g.DrawLines(pen, projectedArc);

					// 5. Малюємо стрілку на кінці дуги
					PointF arrowStart = projectedArc[projectedArc.Length - 1];
					PointF arrowEnd = CalculateArrowEnd(arcPoints[arcPoints.Count - 2], arcPoints[arcPoints.Count - 1], center, rotationX, rotationY, scale);
					DrawArrow(g, pen, arrowStart, arrowEnd);
				}

				// 6. Додаємо підпис з напрямком осі
				using (Font font = new Font("Arial", 8 * scale))
				using (Brush brush = new SolidBrush(Color))
				{
					string axisLabel = GetAxisLabel();
					PointF labelPos = center3D.Project(center, rotationX, rotationY, scale);
					g.DrawString($"{Magnitude} Nm ({axisLabel})", font, brush, labelPos);
				}
			}

			private Point3D CalculateArcPoint3D(Point3D center, float angle)
			{
				// Обчислюємо точку на дузі в 3D просторі
				float angleRad = angle * (float)Math.PI / 180f;
				float x = 0, y = 0, z = 0;

				if (Axis.Y != 0) // Момент навколо осі Y (XZ-площина)
				{					
					y = center.Y + (float)Math.Cos(angleRad) * ArcRadius;
					z = center.Z + (float)Math.Sin(angleRad) * ArcRadius;
					x = center.X;
				}
				else if (Axis.Z != 0) // Момент навколо осі Z (XY-площина)
				{
					x = center.X + (float)Math.Cos(angleRad) * ArcRadius;
					y = center.Y + (float)Math.Sin(angleRad) * ArcRadius;
					z = center.Z;
				}
				else // Момент навколо осі X (YZ-площина)
				{
					y = center.Y + (float)Math.Cos(angleRad) * ArcRadius;
					z = center.Z + (float)Math.Sin(angleRad) * ArcRadius;
					x = center.X;
				}

				return new Point3D(x, y, z);
			}

			private PointF CalculateArrowEnd(Point3D prevPoint, Point3D lastPoint, PointF center, float rotX, float rotY, float scale)
			{
				// Обчислюємо напрямок стрілки в 3D просторі
				Vector3D tangent = new Vector3D(
					lastPoint.X - prevPoint.X,
					lastPoint.Y - prevPoint.Y,
					lastPoint.Z - prevPoint.Z);

				// Нормалізуємо та масштабуємо
				float length = (float)Math.Sqrt(tangent.X * tangent.X + tangent.Y * tangent.Y + tangent.Z * tangent.Z);
				tangent = new Vector3D(
					tangent.X / length * 15f,
					tangent.Y / length * 15f,
					tangent.Z / length * 15f);

				// Створюємо точку для кінця стрілки
				Point3D arrowEnd3D = new Point3D(
					lastPoint.X + tangent.X,
					lastPoint.Y + tangent.Y,
					lastPoint.Z + tangent.Z);

				return arrowEnd3D.Project(center, rotX, rotY, scale);
			}

			private string GetAxisLabel()
			{
				if (Axis.X > 0) return "X+";
				if (Axis.X < 0) return "X-";
				if (Axis.Y > 0) return "Y+";
				if (Axis.Y < 0) return "Y-";
				if (Axis.Z > 0) return "Z+";
				return "Z-";
			}
			private PointF GetPointOnArc(RectangleF rect, float angle)
			{
				float rad = angle * (float)Math.PI / 180f;
				float x = rect.X + rect.Width / 2 + rect.Width / 2 * (float)Math.Cos(rad);
				float y = rect.Y + rect.Height / 2 + rect.Height / 2 * (float)Math.Sin(rad);
				return new PointF(x, y);
			}

			private void DrawArrow(Graphics g, Pen pen, PointF from, PointF to)
			{
				float arrowSize = 8 * pen.Width;
				float angle = (float)Math.Atan2(to.Y - from.Y, to.X - from.X);

				PointF arrowPoint1 = new PointF(
					to.X - arrowSize * (float)Math.Cos(angle - Math.PI / 6),
					to.Y - arrowSize * (float)Math.Sin(angle - Math.PI / 6));

				PointF arrowPoint2 = new PointF(
					to.X - arrowSize * (float)Math.Cos(angle + Math.PI / 6),
					to.Y - arrowSize * (float)Math.Sin(angle + Math.PI / 6));

				g.DrawLine(pen, to, arrowPoint1);
				g.DrawLine(pen, to, arrowPoint2);
			}
		}		

		public class RollerSupport
		{
			public Point3D Position { get; set; }
			public float Radius { get; set; } = 15;
			public Color Color { get; set; } = Color.DarkOrange;

			public RollerSupport(Point3D position)
			{
				Position = position;
			}
			
			public void Render(Graphics g, PointF center, float rotationX, float rotationY, float scale)
			{
				PointF center2D = Position.Project(center, rotationX, rotationY, scale);
				float scaledRadius = Radius * scale;
				float rollerHeight = scaledRadius * 0.3f;

				// Основа опори (з градієнтом)
				using (var path = new GraphicsPath())
				using (var brush = new LinearGradientBrush(
					new PointF(center2D.X - scaledRadius, center2D.Y - scaledRadius),
					new PointF(center2D.X + scaledRadius, center2D.Y + scaledRadius),
					Color.LightGray, Color.DarkGray))
				{
					path.AddEllipse(center2D.X - scaledRadius, center2D.Y - scaledRadius,
								  scaledRadius * 2, scaledRadius * 2);
					g.FillPath(brush, path);
				}

				// Ролики (3 горизонтальні циліндри)
				for (int i = 0; i < 3; i++)
				{
					float yPos = center2D.Y + scaledRadius + (i * rollerHeight * 1.5f);

					using (var path = new GraphicsPath())
					using (var brush = new LinearGradientBrush(
						new PointF(center2D.X - scaledRadius, yPos - rollerHeight),
						new PointF(center2D.X + scaledRadius, yPos + rollerHeight),
						Color.Silver, Color.DimGray))
					{
						path.AddEllipse(center2D.X - scaledRadius, yPos - rollerHeight,
									  scaledRadius * 2, rollerHeight * 2);
						g.FillPath(brush, path);
						g.DrawPath(Pens.DarkSlateGray, path);
					}
				}

				// Вертикальна лінія до балки (з тінню)
				using (var pen = new Pen(Color.DimGray, 2 * scale))
				{
					// Тінь
					g.DrawLine(new Pen(Color.FromArgb(50, Color.Black), 3 * scale),
							 center2D.X + 2, center2D.Y - scaledRadius,
							 center2D.X + 2, center2D.Y - scaledRadius * 2.5f);

					// Основна лінія
					g.DrawLine(pen, center2D.X, center2D.Y - scaledRadius,
							  center2D.X, center2D.Y - scaledRadius * 2.5f);
				}
			}
		}
	

		public class HingeSupport
		{
			public Point3D Position { get; set; }
			public float Radius { get; set; } = 15;
			public Color Color { get; set; } = Color.DarkRed;

			public HingeSupport(Point3D position)
			{
				Position = position;
			}			

			public void Render(Graphics g, PointF center, float rotationX, float rotationY, float scale)
			{
				PointF center2D = Position.Project(center, rotationX, rotationY, scale);
				float scaledRadius = Radius * scale;
				float supportHeight = scaledRadius * 1.5f;

				// 1. Основа шарніра (проста заливка замість PathGradientBrush)
				using (var brush = new SolidBrush(Color.Gold))
				{
					g.FillEllipse(brush,
						center2D.X - scaledRadius,
						center2D.Y - scaledRadius,
						scaledRadius * 2,
						scaledRadius * 2);
				}

				// 2. Деталізація шарніра
				using (var pen = new Pen(Color.DarkGoldenrod, 1.5f * scale))
				{
					// Контур
					g.DrawEllipse(pen,
						center2D.X - scaledRadius,
						center2D.Y - scaledRadius,
						scaledRadius * 2,
						scaledRadius * 2);

					// Хрестоподібні лінії
					g.DrawLine(pen,
						center2D.X - scaledRadius * 0.7f, center2D.Y,
						center2D.X + scaledRadius * 0.7f, center2D.Y);
					g.DrawLine(pen,
						center2D.X, center2D.Y - scaledRadius * 0.7f,
						center2D.X, center2D.Y + scaledRadius * 0.7f);

					// Болтове з'єднання (простий круг)
					g.FillEllipse(Brushes.SteelBlue,
						center2D.X - scaledRadius * 0.2f,
						center2D.Y - scaledRadius * 0.2f,
						scaledRadius * 0.4f,
						scaledRadius * 0.4f);
				}

				// 3. Трикутна підставка (без PathGradientBrush)
				PointF[] triangle = {
		new PointF(center2D.X - scaledRadius * 1.2f, center2D.Y + scaledRadius),
		new PointF(center2D.X + scaledRadius * 1.2f, center2D.Y + scaledRadius),
		new PointF(center2D.X, center2D.Y + scaledRadius + supportHeight)
	};

				using (var brush = new SolidBrush(Color.DarkSlateGray))
				{
					g.FillPolygon(brush, triangle);
				}

				// Контур підставки
				using (var pen = new Pen(Color.Black, 1.5f * scale))
				{
					g.DrawPolygon(pen, triangle);
				}

				// 4. З'єднання з балкою (простий лінійний ефект)
				using (var pen = new Pen(Color.DimGray, 2.5f * scale))
				{
					g.DrawLine(pen,
						center2D.X, center2D.Y - scaledRadius,
						center2D.X, center2D.Y - scaledRadius * 2f);
				}
			}
		}

		public class Vector3D
		{
			public float X { get; set; }
			public float Y { get; set; }
			public float Z { get; set; }

			public Vector3D(float x, float y, float z)
			{
				X = x;
				Y = y;
				Z = z;
			}

			// Оператор множення вектора на скаляр
			public static Vector3D operator *(Vector3D v, float scalar)
			{
				return new Vector3D(v.X * scalar, v.Y * scalar, v.Z * scalar);
			}
		}

		private void балкаToolStripMenuItem_Click(object sender, EventArgs e)
		{			
			Form editForm = new Form
			{
				Text = "Редагування параметрів балки",
				Width = 300,
				Height = 200,
				FormBorderStyle = FormBorderStyle.FixedDialog,
				StartPosition = FormStartPosition.CenterParent
			};

			AddNumericControl(editForm, "Довжина (X):", 20, 30, (int)GetBeamLength(), 50, 2000);
			AddNumericControl(editForm, "Висота (Y):", 20, 60, (int)beamWidth, 5, 100);
			AddNumericControl(editForm, "Ширина (Z):", 20, 90, (int)beamHeight, 5, 100);

			Button saveButton = new Button
			{
				Text = "Зберегти",
				DialogResult = DialogResult.OK,
				Location = new Point(100, 130),
				Size = new Size(100, 30)
			};
			saveButton.Click += (s, args) =>
			{
				// Оновлюємо параметри балки
				float newLength = (float)((NumericUpDown)editForm.Controls[1]).Value;
				beamEnd = new Point3D(beamStart.X + newLength, beamEnd.Y, beamEnd.Z);
				beamWidth = (float)((NumericUpDown)editForm.Controls[3]).Value;
				beamHeight = (float)((NumericUpDown)editForm.Controls[5]).Value;
				
				bool wasDeflectionVisible = showDeflection;
				float savedModulus = currentElasticityModulus;				

				InitializeScene();

				// Відновлюємо стан
				showDeflection = wasDeflectionVisible;
				currentElasticityModulus = savedModulus;

				if (scene.Beam != null)
				{
					scene.Beam.SetShowDeflection(wasDeflectionVisible);
					scene.Beam.SetElasticityModulus(savedModulus);
				}

				pictureBox1.Invalidate();
			};
			editForm.Controls.Add(saveButton);

			editForm.ShowDialog(this);
		}

		// Метод для додавання числових контролів (залишається незмінним)
		private void AddNumericControl(Control parent, string labelText, int x, int y, int value, int min, int max)
		{			
			Label label = new Label()
			{
				Text = labelText,
				Location = new Point(x, y),
				AutoSize = true
			};
			parent.Controls.Add(label);

			NumericUpDown numeric = new NumericUpDown()
			{
				Location = new Point(x + 150, y - 3),
				Width = 100,
				Minimum = min,
				Maximum = max,
				Value = value
			};
			parent.Controls.Add(numeric);
		}

		private void зосередженаСилаToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Form editForm = new Form
			{
				Text = "Редагування сили",
				Width = 350,
				Height = 200,
				FormBorderStyle = FormBorderStyle.FixedDialog,
				StartPosition = FormStartPosition.CenterParent
			};

			// Позиція сили
			AddNumericControl(editForm, "Позиція (X):", 20, 30, (int)forcePosition.X, 0, (int)beamEnd.X);

			// Величина сили
			AddNumericControl(editForm, "Величина (N):", 20, 60, (int)forceMagnitude, 0, 1000);

			// Кут сили (-90° до +90°)
			AddNumericControl(editForm, "Кут (градуси):", 20, 90, (int)0, -90, 90);

			Button saveButton = new Button
			{
				Text = "Зберегти",
				Location = new Point(120, 130),
				Size = new Size(100, 30)
			};
			saveButton.Click += (s, args) =>
			{
				forcePosition = new Point3D((float)((NumericUpDown)editForm.Controls[1]).Value, 0, -10);
				forceMagnitude = (float)((NumericUpDown)editForm.Controls[3]).Value;
				float newAngle = (float)((NumericUpDown)editForm.Controls[5]).Value;

				// Оновлюємо силу
				scene.ClearForces();
				Force newForce = new Force(forcePosition, forceMagnitude, newAngle);
				scene.AddForce(newForce);

				pictureBox1.Invalidate();
				editForm.Close();
			};
			editForm.Controls.Add(saveButton);

			editForm.ShowDialog(this);
		}

		public class BeamDeflectionCalculator
		{
			public static Vector3D CalculateDeflection3D(
		   float x, float L, float E,
		   float Iy, float Iz,
		   List<Force> forces,
		   List<Moment> moments,
		   bool isFixedSupport) 
			{
				float wy = 0; // Прогин у площині XZ (від Y-компоненти сили)
				float wz = 0; // Прогин у площині XY (від Z-компоненти сили)

				foreach (var force in forces)
				{
					float a = force.ApplicationPoint.X;
					float Fy = force.Magnitude * force.Direction.Y;
					float Fz = force.Magnitude * force.Direction.Z;
					float b = L - a;					

					if (isFixedSupport)
					{
						// Розрахунки для защемленої балки (між стінами)

						if (isFixedSupport)
						{							
						
							if (x <= a)
							{
								wy += Fy * b * b * x * x * (3 * a * L - 2 * a * x - L * x) / (6 * E * Iy * L * L * L);
								wz += Fz * b * b * x * x * (3 * a * L - 2 * a * x - L * x) / (6 * E * Iz * L * L * L);
							}
							else
							{
								wy += Fy * a * a * (L - x) * (L - x) * (3 * b * L - 2 * b * (L - x) - L * (L - x)) / (6 * E * Iy * L * L * L);
								wz += Fz * a * a * (L - x) * (L - x) * (3 * b * L - 2 * b * (L - x) - L * (L - x)) / (6 * E * Iz * L * L * L);
							}
						}
					}
					else
					{
						// Розрахунки для балки на шарнірах 
						if (x <= a)
						{
							wy += Fy * x * (L - a) * (L * L - x * x - (L - a) * (L - a)) / (6 * E * Iy * L);
							wz += Fz * x * (L - a) * (L * L - x * x - (L - a) * (L - a)) / (6 * E * Iz * L);
						}
						else
						{
							wy += Fy * a * (L - x) * (2 * L * x - x * x - a * a) / (6 * E * Iy * L);
							wz += Fz * a * (L - x) * (2 * L * x - x * x - a * a) / (6 * E * Iz * L);
						}
					}
				}
				// Розрахунок від моментів				
				foreach (var moment in moments)
				{
					float a = moment.ApplicationPoint.X;
					float My = moment.Magnitude * moment.Axis.Y;
					float Mz = moment.Magnitude * moment.Axis.Z;

					if (isFixedSupport)
					{
						// Для балки з двома защемленими кінцями
						float termY = My / (6 * E * Iy * L * L);
						float termZ = Mz / (6 * E * Iz * L * L);

						if (x <= a)
						{
							wy += termY * x * x * (3 * a * L - 2 * a * x - L * x);
							wz += termZ * x * x * (3 * a * L - 2 * a * x - L * x);
						}
						else
						{
							wy += termY * a * a * (3 * x * L - 2 * x * x - L * x);
							wz += termZ * a * a * (3 * x * L - 2 * x * x - L * x);
						}
					}
					else
					{
						// Для балки на шарнірних опорах
						float commonFactorY = My / (E * Iy * L);
						float commonFactorZ = Mz / (E * Iz * L);

						if (x <= a)
						{
							wy += commonFactorY * x * (L - a) * (L + a - x) / 2;
							wz += commonFactorZ * x * (L - a) * (L + a - x) / 2;
						}
						else
						{
							wy += commonFactorY * a * (L - x) * (L + x - a) / 2;
							wz += commonFactorZ * a * (L - x) * (L + x - a) / 2;
						}
					}
				}
				return new Vector3D(0, wy, wz);
			}
		}

		private void показатиСховатиДеформаціюToolStripMenuItem_Click(object sender, EventArgs e)
		{				
			if (scene?.Beam == null) return;

			showDeflection = !showDeflection; // Оновлюємо стан
			scene.Beam.SetShowDeflection(showDeflection);

			// Оновлюємо текст меню
			показатиСховатиДеформаціюToolStripMenuItem.Text = showDeflection
				? "Сховати деформацію"
				: "Показати деформацію";

			pictureBox1.Invalidate();
		}

		private void властивостіМатеріалуToolStripMenuItem_Click(object sender, EventArgs e)
		{			
			if (scene == null || scene.Beam == null)
			{
				MessageBox.Show("Балка не ініціалізована!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			// Створюємо форму для вибору матеріалу
			Form materialForm = new Form
			{
				Text = "Властивості матеріалу",
				Width = 300,
				Height = 200,
				FormBorderStyle = FormBorderStyle.FixedDialog,
				StartPosition = FormStartPosition.CenterParent
			};

			// Додаємо вибір матеріалу
			ComboBox materialCombo = new ComboBox
			{
				Location = new Point(20, 20),
				Width = 250,
				DropDownStyle = ComboBoxStyle.DropDownList
			};
			materialCombo.Items.AddRange(new object[] {
		"Сталь (E=200 GPa)",
		"Алюміній (E=70 GPa)",
		"Деревина (E=10 GPa)",
		"Користувацький..."
	});
			materialCombo.SelectedIndex = 0;
			materialForm.Controls.Add(materialCombo);

			// Додаємо поле для власного значення модуля пружності
			Label customLabel = new Label
			{
				Text = "Модуль пружності (GPa):",
				Location = new Point(20, 60),
				AutoSize = true,
				Visible = false
			};
			materialForm.Controls.Add(customLabel);

			NumericUpDown customModulus = new NumericUpDown
			{
				Location = new Point(20, 80),
				Width = 100,
				Minimum = 1,
				Maximum = 1000,
				Value = 200,
				Visible = false
			};
			materialForm.Controls.Add(customModulus);

			// Обробник зміни вибору матеріалу
			materialCombo.SelectedIndexChanged += (s, args) =>
			{
				bool isCustom = materialCombo.SelectedIndex == 3;
				customLabel.Visible = isCustom;
				customModulus.Visible = isCustom;
			};

			// Кнопка збереження
			Button saveButton = new Button
			{
				Text = "Зберегти",
				DialogResult = DialogResult.OK,
				Location = new Point(100, 120),
				Size = new Size(100, 30)
			};
			saveButton.Click += (s, args) =>
			{
				switch (materialCombo.SelectedIndex)
				{
					case 0: currentElasticityModulus = 2.0e5f; break;  // Сталь
					case 1: currentElasticityModulus = 0.7e5f; break;  // Алюміній
					case 2: currentElasticityModulus = 0.1e5f; break;  // Деревина
					case 3: currentElasticityModulus = (float)customModulus.Value * 1e3f; break;
				}

				if (scene?.Beam != null)
				{
					scene.Beam.SetElasticityModulus(currentElasticityModulus);
				}

				scene.Beam.SetElasticityModulus(currentElasticityModulus);
				pictureBox1.Invalidate();
				materialForm.Close();
			};
			materialForm.Controls.Add(saveButton);

			materialForm.ShowDialog(this);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == (Keys.Control | Keys.D))
			{
				показатиСховатиДеформаціюToolStripMenuItem_Click(null, null);
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void балкаМіжСтінамиToolStripMenuItem_Click(object sender, EventArgs e)
		{
			showWallsInsteadOfSupports = true;
			InitializeScene();
		}

		private void балкаМіжШарнірамиToolStripMenuItem_Click(object sender, EventArgs e)
		{
			showWallsInsteadOfSupports = false;
			InitializeScene();
		}

		private void моментToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (scene == null || scene.Beam == null) return;

			Form editMomentForm = new Form()
			{
				Text = "Редагування моменту",
				Width = 350,
				Height = 220,
				FormBorderStyle = FormBorderStyle.FixedDialog,
				StartPosition = FormStartPosition.CenterParent
			};

			// Позиція моменту
			AddNumericControl(editMomentForm, "Позиція (X):", 20, 20,
				(int)momentPosition.X, 0, (int)beamEnd.X);

			// Величина моменту
			AddNumericControl(editMomentForm, "Величина (Nm):", 20, 60,
				(int)momentValue, -1000, 5000);
			
			Button saveButton = new Button()
			{
				Text = "Зберегти",
				DialogResult = DialogResult.OK,
				Location = new Point(120, 130),
				Size = new Size(100, 30)
			};
			saveButton.Click += (s, args) =>
			{
				momentPosition = new Point3D(
					(float)((NumericUpDown)editMomentForm.Controls[1]).Value, 0, 0);
				momentValue = (float)((NumericUpDown)editMomentForm.Controls[3]).Value;

				// Оновлюємо сцену
				scene.ClearMoments();
				scene.AddMoment(new Moment(momentPosition, momentAxis, momentValue));
				pictureBox1.Invalidate();
			};
			editMomentForm.Controls.Add(saveButton);

			editMomentForm.ShowDialog(this);
		}

		
		// Метод для оновлення стану пунктів меню
	private void UpdateMenuItems()
		{
			foreach (ToolStripMenuItem item in menuStrip1.Items)
			{
				if (item.Text == "Вигляд")
				{
					foreach (ToolStripMenuItem subItem in item.DropDownItems)
					{
						if (subItem.Text == "Сила")
						{
							subItem.DropDownItems[0].Enabled = !forceExists;
							subItem.DropDownItems[1].Enabled = forceExists;
						}
						else if (subItem.Text == "Момент")
						{
							subItem.DropDownItems[0].Enabled = !momentExists;
							subItem.DropDownItems[1].Enabled = momentExists;
						}
					}
				}
			}
		}

		private void додатиСилуToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!forceExists)
			{
				scene.AddForce(new Force(forcePosition, forceMagnitude, 0));
				forceExists = true;
				pictureBox1.Invalidate();
				UpdateMenuItems();
			}
		}

		private void прибратиСилуToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (forceExists)
			{
				scene.ClearForces();
				forceExists = false;
				pictureBox1.Invalidate();
				UpdateMenuItems();
			}
		}

		private void додатиМоментToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!momentExists)
			{
				scene.AddMoment(new Moment(momentPosition, momentAxis, momentValue));
				momentExists = true;
				pictureBox1.Invalidate();
				UpdateMenuItems();
			}
		}

		private void прибратиМоментToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (momentExists)
			{
				scene.ClearMoments();
				momentExists = false;
				pictureBox1.Invalidate();
				UpdateMenuItems();
			}
		}
	}
}
