# **GEMINI-CORE: Retro-Grid Engine (RGE)**

## **🌌 Void Dig 🌌**

**GEMINI-CORE** is a modular, retro grid-based game engine focused on dynamic physics puzzle-action. It utilizes the Procedural Anomaly Generator (PAG) for creating infinite, unique, and mathematically guaranteed-solvable levels, specializing in complex rockfall and terrain manipulation mechanics.

The engine's debut title, **Void Dig**, challenges players to navigate collapsing mines and manipulate unstable physics to harvest valuable resources across infinite levels.

## **🕹️ Game Concept: Void Dig**

You are a Void Diver, tasked with extracting hyper-volatile **Aether Shards** from the cores of unstable, low-gravity celestial bodies. Success depends on mastering the unique, volatile environmental mechanics.

### **Key Elements**

1. **The Void Diver:** Your character can dig through soft earth, push certain blocks, and utilize special items.  
2. **Aether Shards (Goal):** The required collectibles. Collecting the required quota opens the exit portal to the next level.  
3. **Phase Gravel (Dynamic Terrain):** This unstable earth shifts state on a global timer (e.g., every 5 seconds) or when the player interacts with a "Quantum Switch."  
   * **Solid State:** Behaves like normal earth; can be dug through.  
   * **Intangible State:** Becomes transparent and non-solid; rocks/gems fall straight through it. Timing your passage or rock movements through intangible paths is critical.  
4. **Quantum Bombs (Physics Manipulators):** Collectible, single-use items that, when deployed, temporarily reverse local gravity for a short duration (e.g., 3 seconds). This allows rocks to fall *up* or, in some level configurations, diagonally, offering unique path-clearing opportunities and hazards.  
5. **Corrupted Minerals (Hazards):** Standard rocks and gems that fall under gravity (or reversed gravity). Crushing the player is the primary hazard.  
6. **Plasma Wasps (Enemies):** Simple, maze-following enemies that can only traverse open tunnels.

## **⚙️ Engine Architecture: Retro-Grid Engine (RGE)**

GEMINI-CORE is designed for high performance on a fixed 16x16 or 32x32 tile grid, built on a modular, component-based design.

### **Core Components**

| Component | Description | Functionality |
| :---- | :---- | :---- |
| **GridManager** | The central authority for the game map. | Manages the 2D array of cells, handles tile property lookups (Solid, Diggable, Collectible, Dynamic). |
| **PhysicsHandler** | The heart of the "Dash" style gameplay. | Iterates over the grid (top-down) once per game tick to check for gravity/movement rules: Rock support, free fall calculation, player collision, and "rolling" physics. |
| **StateScheduler** | Handles all time-based dynamic elements. | Specifically manages the global timer for **Phase Gravel** state changes and the cooldown/duration for **Quantum Bomb** effects. |
| **InputController** | Maps keyboard/gamepad input to GridManager movements and item usage. | Ensures smooth, responsive 8-directional movement that snaps to the grid. |
| **Procedural Anomaly Generator (PAG)** | The core of the infinite level system. | Generates and verifies level layouts based on defined difficulty parameters. |

## **♾️ Infinite Levels: Procedural Anomaly Generator (PAG)**

The PAG is built around ensuring that every generated level is both solvable and unique. It uses a 3-stage process to guarantee playability: **Structure, Placement, Verification.**

### **1\. Structure Generation (The Dungeon Blueprint)**

The PAG creates the high-level pathing and environment layout:

* **Voronoi Tunnels:** Lays down key "pillars" or "safe zones" and connects them using randomized paths, creating the primary network of tunnels.  
* **Flow Channels:** Carves out specific areas marked as \[FLOW\_CHANNEL\] where heavy concentrations of rocks and dynamic terrain (Phase Gravel) are placed, forming the main hazard areas.  
* **Boundary Layer:** Ensures the entire playable area is enclosed by a solid, undiggable core material.

### **2\. Element Placement (Filling the Voids)**

Elements are placed based on a "Density Map" derived from the current difficulty setting:

* **Aether Shards:** Placed sparsely, often requiring interaction with dynamic elements to reach.  
* **Corrupted Minerals (Rocks):** Clustered heavily in Flow Channels, ensuring there is enough empty space beneath them to allow for satisfying cascades and falling traps.  
* **Phase Gravel:** Placed strategically to cut off or provide temporary access to objectives.  
* **Enemies (Plasma Wasps):** Spawns controlled by simple pathfinding logic, ensuring they patrol viable routes without getting stuck.

### **3\. Solvability Verification (The Playability Test)**

This is the most critical stage. The PAG must guarantee that the level is winnable:

1. \**A* Pathfinding Check:\*\* A standard A\* algorithm is run from the player's start point to the exit portal to ensure a basic structural path exists.  
2. **Resource Check:** It verifies that a path exists that allows the player to reach *all* required Aether Shards without soft-locking.  
3. **Dynamic Simulation:** The verification runs a simulated, high-speed playthrough where the Phase Gravel switches state randomly. If a path can be completed *at least once* across multiple simulated runs, the level is deemed "dynamically solvable."  
4. **No Traps at Start:** Ensures the player is not immediately crushed by falling rocks upon spawning.

## **🚀 Getting Started**

**Clone the Repository:**  
   git clone \[https://github.com/organizations/dreamvision-dev/GEMINI-CORE.git\](https://github.com/organizations/dreamvision-dev/GEMINI-CORE.git)  
   cd GEMINI-CORE

## **🤝 Contribution**

We welcome contributions\! Please check the CONTRIBUTING.md file for guidelines on submitting pull requests and reporting bugs.

### **Core Development Areas:**

* Refining the PhysicsHandler to eliminate potential clipping/soft-lock bugs.  
* Optimizing the PAG's Dynamic Simulation for faster level loading.  
* Expanding the 16x16 pixel art tile set and sprite sheet.

## **✉️ Contact and Attribution**

This project is maintained by **Digital Vision**, led by **Mehmet T. AKALIN**.

* **Maintainer GitHub:** [https://github.com/makalin](https://github.com/makalin)  
* **Company Website:** [https://dv.com.tr](https://dv.com.tr)  
* **X (Twitter):** [https://x.com/makalin](https://x.com/makalin)  
* **LinkedIn:** [https://www.linkedin.com/in/makalin/](https://www.linkedin.com/in/makalin/)

## **📜 License**

Distributed under the MIT License. See LICENSE for more information.
