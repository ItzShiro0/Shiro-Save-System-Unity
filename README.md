# Shiro Save System

A lightweight, secure, and autonomous save system for Unity based on ScriptableObjects.

## Features

* **RAM Protection (XOR Anti-Cheat):** Secures variables in memory to prevent scanning and manipulation via tools like Cheat Engine.
* **Disk Protection (SHA-256 Validation):** Generates a cryptographic hash for `.json` save files. Any manual tampering with the save file will result in an automatic reset to default values.
* **ScriptableObject Schemas:** Safe initialization using default values. Prevents `NullReferenceException` when loading empty or new saves.
* **Custom Editor Inspector:** Clean UI with built-in protection against accidental asset renaming to keep the database stable.

## Quick Start

### 1. Create a Save Schema
1. In Unity, go to `Tools -> Save System -> Create New Schema`.
![Inspector Demo](Media/converted.gif)
2. Enter a unique Save ID (e.g., `gameplay_save`) and click **Save Name**.
![Inspector Demo](Media/converted(1).gif) 
3. Add your default variables using the Custom Inspector.
![Inspector Demo](Media/converted(2).gif) 

### 2. Usage in Code

Interact with the save system directly via the static `SaveSystem` class without looking for components on the scene.

```csharp
int currentCoins = SaveSystem.GetInt("gameplay_save", "Coins"); // Read a value (returns default from Schema if it's a new game)

SaveSystem.SetInt("gameplay_save", "Coins", currentCoins + 100); // Update a value in RAM

SaveSystem.Save("gameplay_save"); // Save progress to disk physically

SaveSystem.Reset("gameplay_save"); // Reset progress to default Schema values
