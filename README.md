![image](https://github.com/user-attachments/assets/d8c04126-3c0c-4052-aa5f-044ffe81fdbd)# 🎮 2D Player Controller for Unity

This repository contains a modular and extendable 2D Player Controller system built in Unity. It includes support for smooth movement, a state-based controller, and combat functionality with animations and sprite setup.

> 🧪 Originally developed as a core system for a 2D action game prototype. Shared for learning and reuse purposes.

---

## 🧠 Features

- 🎮 **Player Movement**  
  - Walk/run with physics-based motion  
  - Handles horizontal input and sprite flipping

- ⚔️ **Combat System**  
  - Light attack input handling  
  - Modular combat logic via `PlayerCombat.cs`

- 🔄 **State Management**  
  - Finite state machine for handling movement, idle, and attack states  
  - Centralized in `PlayerStateController.cs`

- 🎞️ **Animations**  
  - Integrated with Unity’s Animator Controller (`PlayerAnimationController_.controller`)  
  - Includes `.anim` files for light attacks, loops, and slashes

- 🎨 **Sprite + Animation Assets**  
  - `.aseprite` source files for attack and movement sprites  
  - Ready for iteration or sprite rework

---

## 📁 File Structure

```plaintext
Player/
├── PlayerController.cs          # Main movement logic
├── PlayerCombat.cs              # Combat logic for attacking
├── PlayerStateController.cs     # State management and transitions
├── PlayerMovement.cs            # Low-level input and motion
├── *.anim, *.controller         # Animation assets
├── *.aseprite                   # Sprite packs for visuals


---

## 🚀 Getting Started

1. Drop the `Player/` folder into your Unity project’s `Assets/` directory.
2. Attach the following scripts to your Player GameObject:
   - `PlayerController`
   - `PlayerMovement`
   - `PlayerCombat`
   - `PlayerStateController`
3. Link the `PlayerAnimationController_.controller` to the Animator.
4. Add animations and configure transitions inside the Animator.
5. Customize as needed!

---

## 📌 Notes

- Built with Unity’s classic input system.
- Designed for modularity — each script handles a specific piece of player behavior.
- Sprite and animation files are provided as working references, not final art.

---

## 📜 License

MIT — feel free to use, modify, and learn from this controller system. Please credit if reused in public or commercial projects.
