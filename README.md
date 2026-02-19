# 🎮 Zero Drag

**Zero Drag** isn't just another FPS; it's a journey of building, breaking, and learning. Developed in Unity, this project represents my transition from a "tutorial follower" to a "system architect."

---

## 🚀 The Journey So Far

This project started with a simple idea: *How do I make a movement system that feels real?* From there, it snowballed into a full-scale development effort. I didn't want to use templates—I wanted to code every logic block to understand the "why" behind the mechanics.

### ✨ Core Features
- **Fluid Player Movement:** Hand-coded physics-based movement including sprinting and responsive controls.
- **Smart Enemy AI:** Enemies that don't just stand there—they have health systems, reaction logic, and basic AI behaviors.
- **Dynamic Weapon Systems:** Integrated weapon pickup mechanics and shooting logic.
- **Narrative Infrastructure:** Built-in systems for cutscenes and story flow.

---

## 🛠️ The "Bug Hunter" Log (Real Problems, Real Fixes)

Building a game is 10% coding and 90% debugging. Here are some of the biggest hurdles I've cleared:

1.  **The "Pink Screen" Saga:** Fixed shader compatibility issues where models appeared bright pink due to mismatched render pipelines.
2.  **Input Ghosting:** Resolved the "Input Axis not setup" errors by re-configuring the Unity Input Manager and merging old/new input systems.
3.  **NPC "Frozen" Reactions:** Fixed a bug where NPCs wouldn't react to being shot. Had to deep-dive into the `GunShot.cs` logic to ensure collision triggers were clean.
4.  **UI Ghosting:** Debugged 7-segment displays and ranking boards that were unreadable or misaligned.

---

## 🧠 Lessons Learned

- **Professionalism over Hacks:** I learned that "Commit Farming" (making 10 fake pushes to look busy) doesn't help. Real progress is measured in clean code and solved problems.
- **Architecture Matters:** Organization is key. I refactored the entire project structure from a messy pile into clean folders: `Player`, `Enemy`, `Weapon`, `Manager`.
- **Clean Code is King:** Removing redundant `Debug.Log` calls and messy comments isn't just about "cleaning up"—it's about making the code readable for my future self and my team.

---

## 🔨 Getting Started

1.  Open **Unity Hub**.
2.  Import the project folder.
3.  Ensure your **Input Manager** settings are set to "Both" (New and Old systems).
4.  Hit **Play** and experience the drag-free movement of *Zero Drag*.

---

*“Built to learn, built to ship.”*
