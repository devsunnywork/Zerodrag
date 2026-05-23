# 🕶️ ZERO DRAG — A Crime Thriller FPS
Developed Solo by **Sunny Rajput** (Lead Architect & Programmer)

[![Zero Drag Pitch](https://img.shields.io/badge/Official-Website-00f0ff?style=for-the-badge&logo=html5)](https://zerodrag.netlify.app/)
[![Read GDD](https://img.shields.io/badge/Documentation-GDD-ff2e51?style=for-the-badge&logo=markdown)](./GDD.md)

---

> [!IMPORTANT]
> **READ THE GAME DESIGN DOCUMENT (GDD) FIRST:**
> The complete architectural blueprint, kinematic mathematical equations, weapon progression metrics, dialogue layouts, and development milestones are documented in **[GDD.md](./GDD.md)**. Please refer to it for a detailed technical dive into the Unity implementation.

---

## 📖 The Core Concept & Story

In the neon-drenched, rain-slicked underbelly of a modern Indian metropolis, hesitation is death.

**Zero Drag** follows **Rupesh**, a completely ordinary citizen pushed to the absolute edge by debt and desperation. After accepting a simple warehouse package drop-off from **Salim Bhai**, Rupesh witnesses a high-level mob execution. Cornered by corrupt police dragnets and lethal syndicate hit squads, Rupesh undergoes a dark psychological and physical evolution—transforming from a normal citizen into a cold-blooded, high-agility gangster.

The name **Zero Drag** represents Rupesh's ultimate lethal state: operating with *zero hesitation, zero emotional delay, and absolute frictionless momentum*.

---

## ⚙️ Technical Architecture Overview

The codebase is built entirely on modern decoupled principles in Unity, utilizing custom physics and singleton managers rather than dragging heavy default engines.

1. **Frictionless WASD Locomotion (Kinematics)**
   - Driven manually through Unity's `CharacterController`. Acceleration and deceleration are calculated dynamically to eliminate sliding latency.
   - **Kinematic Jump Equation:** Vertical leap velocity derived from displacement using $u = \sqrt{h \times -2 \times g}$.
   - **Exorcist Camera Clamping:** Local mouse look yaw rotates the root body, while pitch is strictly locked between exactly $-90^{\circ}$ and $+90^{\circ}$ to prevent neck flips.

2. **The "Fake Switch" Fracture Physics**
   - Traditional physics ragdolls create performance bottlenecks. On lethal hits, enforcer thugs are instantly disabled and replaced with duplicates of their fractured pre-broken mesh models.
   - Localized radial explosion forces (`AddExplosionForce(500f, transform.position, 5f)`) blow the individual fragments outward dynamically across 3D space.

3. **Decoupled Singletons (`ScoreManager`)**
   - The game's economy (Cash, territory indicators, rival syndicate threat states) is calculated dynamically.
   - On scene load, the singleton scans for tagged agents using `GameObject.FindGameObjectsWithTag("Enemy")`, removing heavy editor inspector links and preventing sync delays.

4. **Cinematic Dialogue Typewriter**
   - Features speaker tag identifiers, keyboard skipping, and custom action callbacks (to trigger safehouse gate triggers and spawn ambient ambush events).

---

## 🗺️ Project Workspace Navigation Map

Explore the design layouts, story acts, and front-end showcases here:

*   📖 **[GDD.md](./GDD.md) (Game Design Document):** The holy grail of the project. Contains physics formulas, codebase architecture, inventory scripts, and directory structures.
*   🎭 **[StoryDesign.md](./StoryDesign.md) (Story & Script):** Details the 10-scene opening cinematic script between Salim Bhai, Rupesh, and Vikram, along with the act branches.
*   🖥️ **[Website/index.html](./Website/index.html) (Landing Web Showcase):** The official marketing site. Features a glassmorphic capsule menu, a live consequence simulator, custom responsive dossiers, and tabbed weapon drawers.

---

## 🔨 How to Get Started

### 1. Previews & Marketing Site
To inspect the gorgeous web landing page, visit the live deployment at **[zerodrag.netlify.app](https://zerodrag.netlify.app/)** or open the local source file **[index.html](./Website/index.html)** in a browser. It is fully interactive!

### 2. Launching in Unity Editor
1. Open **Unity Hub** and add the project root directory.
2. Select **Unity 2022.3+ (URP)**.
3. Open the main scene under `Assets/Scenes/RailwaysWarehouse.unity`.
4. Ensure your project inputs under **Input Manager** are configured to support both old axis systems and high-mobility layouts.
5. Click **Play** to test Rupesh's frictionless locomotion loop!

---

*“Human reaction time is inefficient. In the syndicate, we removed the drag.”*
