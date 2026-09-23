# 🔴 Mars FPS — First Person Shooter

> A first-person shooter set on Mars, focused on combat, exploration, missions and modular gameplay systems.

![Unity](https://img.shields.io/badge/Unity-000000?style=for-the-badge&logo=unity&logoColor=white)
![C%23](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![GitHub](https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white)

---

## 🎮 About the Project

**Mars FPS** is a first-person shooter set on the hostile surface of Mars.

The player takes on missions, explores the environment, interacts with objects, fights different types of enemies and manages their equipment while progressing through the game.

The project was created with a strong focus on building **modular and reusable gameplay systems**, rather than relying on hardcoded mechanics.

The game includes systems for combat, enemies, weapons, quests, dialogue, localization, interaction, progression, saving/loading and UI.

---

## 🌌 Core Gameplay

The main gameplay loop combines:

| Feature | Description |
|---|---|
| 🔫 Combat | First-person shooting and combat against various enemies |
| 👾 Enemies | Different enemy types with individual behaviour and combat logic |
| 🎯 Missions | Quest-based objectives and progression |
| 🗺️ Exploration | Exploring the Martian environment and discovering interactive elements |
| 🎒 Equipment | Weapons and inventory management |
| 💬 Dialogue | Dialogue system for interacting with characters and progressing through missions |
| 🌍 Localization | Support for multiple languages |
| 📈 Progression | Systems responsible for player progression and unlocking content |
| 🖥️ UI | Gameplay HUD, menus and other user interfaces |

---

# ⚙️ Systems

The project consists of multiple interconnected gameplay systems.

## 🔫 Combat System

The combat system is responsible for handling the core FPS gameplay.

It includes:

- weapon handling
- shooting
- damage calculation
- health management
- different damage interactions
- enemy combat behaviour
- player combat feedback

The systems are designed so that weapons, characters and other gameplay elements can interact with the same underlying health and damage logic.

---

## ❤️ Health & Damage

A dedicated health and damage system handles damage interactions between gameplay entities.

It provides a common foundation for:

- player health
- enemy health
- damage sources
- death handling
- damage processing
- combat feedback

This allows different gameplay systems to communicate with each other without being tightly coupled.

---

## 👾 Enemy System

Enemies have their own behaviour and combat logic.

The system is responsible for:

- detecting the player
- reacting to the player's actions
- choosing appropriate behaviour
- attacking
- taking damage
- handling death
- switching between different behavioural states

Different enemies can use the same underlying architecture while having their own behaviour and configuration.

---

## 🔫 Weapons & Equipment

The weapon and equipment systems allow the player to interact with different gameplay items.

The system handles things such as:

- weapon management
- shooting
- ammunition
- equipment
- item interactions
- switching between available equipment

Weapons are implemented as gameplay systems rather than being directly tied to a single scene or level.

---

## 💬 Dialogue System

The project contains a dialogue system used for conversations and story-related interactions.

It supports:

- dialogue sequences
- branching conversations
- dialogue progression
- integration with quests
- localization
- UI presentation

Dialogue events can also be connected to other gameplay systems, allowing conversations to influence the current game state.

---

## 🌍 Localization

The game includes a localization system for supporting multiple languages.

Text used by different systems can be localized without having to modify the underlying gameplay logic.

Localization is integrated with systems such as:

- dialogues
- quests
- UI
- menus
- gameplay messages

---

## 🎯 Quest System

The quest system manages mission objectives and player progression.

It supports different types of objectives and allows quests to interact with other systems.

Examples include:

- reaching a location
- interacting with objects
- defeating enemies
- collecting items
- completing specific actions

Quests can also trigger events in other systems, connecting gameplay objectives with the rest of the game.

---

## 🖱️ Interaction System

The interaction system allows the player to interact with objects in the environment.

It can be used for different types of interactions, such as:

- doors
- pickups
- switches
- interactive objects
- mission-related objects
- environmental elements

The goal is to provide a common interaction layer that can be reused across different gameplay objects.

---

## 💾 Save / Load System

The game contains a save/load system responsible for persisting the player's progress.

It can store information related to the current game state, allowing the player to continue their progress after restarting the game.

The system is designed to communicate with other gameplay systems instead of having each system implement its own saving logic.

---

## 🖥️ UI & Menus

The project includes a complete UI structure covering both gameplay and meta-game systems.

### Gameplay UI

- health
- ammunition
- objectives
- interaction prompts
- gameplay notifications

### Menus

- Main Menu
- Settings
- Pause Menu
- other game-related screens

The UI is connected to the underlying gameplay systems and reacts to changes in the game state.

---

## ⚙️ Settings

The project also includes a settings system responsible for configuring different aspects of the game.

Examples include:

- graphics settings
- audio settings
- gameplay options
- input-related settings
- localization settings

Settings are persisted between game sessions.

---

## 📈 Progression Systems

The game contains several systems responsible for tracking player progression.

Progression can affect different parts of the game, including:

- completed quests
- unlocked content
- equipment
- player state
- story progression
---
