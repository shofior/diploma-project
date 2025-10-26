Project Overview

This Windows Forms application simulates a beam under transverse bending.
It provides a 3D visualization of the beam, allows the user to apply concentrated forces and moments, choose support types, and display the resulting deflection.

The 3D rendering and deformation visualization are implemented manually using GDI+ without external graphics libraries.

Features:
Real-time 3D visualization of the beam,
Switch between fixed walls and support-based beam types,
Adjust beam geometry (length, width, height),
Choose material (steel, aluminum, wood, or custom modulus of elasticity),
Add or remove concentrated forces and moments,
Display or hide beam deformation under load.

Interactive control:
Rotate with mouse drag,
Zoom with mouse wheel,
Toggle deformation (Ctrl + D),
Implemented Calculations.

The app calculates beam deflection based on classical elasticity theory:
Fixed beam (between rigid walls),
Simply supported beam (on hinge and roller supports).

Formulas account for:
Concentrated loads (Fy, Fz),
Moments (My, Mz),
Elastic modulus (E),
Moments of inertia (Iy, Iz).

Tech Stack:

Language: C#

Framework: .NET (Windows Forms)

Graphics: GDI+ (System.Drawing)

IDE: Visual Studio

Preview:

<img width="454" height="347" alt="image" src="https://github.com/user-attachments/assets/6869d5d5-3aad-4ac1-b659-8d8ed21099ad" /><br><br>

<img width="474" height="236" alt="image" src="https://github.com/user-attachments/assets/efa5a2d1-d358-4524-9665-463b6685b14e" /><br><br>

<img width="404" height="157" alt="image" src="https://github.com/user-attachments/assets/92485b2d-1f0b-4be5-87c1-1a5011bd9193" /><br><br>

<img width="390" height="220" alt="image" src="https://github.com/user-attachments/assets/7493ff35-b3d0-4621-a30c-50451dd96508" /><br><br>

<img width="444" height="231" alt="image" src="https://github.com/user-attachments/assets/b34044b4-0308-48b1-81b1-548dacf5d88a" /><br><br>

<img width="444" height="231" alt="image" src="https://github.com/user-attachments/assets/60b58aaf-eca0-45d6-ad93-5b23eaea431d" /><br><br>

<img width="461" height="190" alt="image" src="https://github.com/user-attachments/assets/7c0d3fdb-c19d-46f0-88ef-a889b638467b" /><br><br>

<img width="445" height="190" alt="image" src="https://github.com/user-attachments/assets/5ec49b00-bebb-4d6a-9258-7ebf12e6801f" /><br><br>

