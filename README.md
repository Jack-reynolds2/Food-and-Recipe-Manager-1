# Food & Recipe Manager

A C# .NET 8 console application for managing ingredients and recipes using a structured, layered architecture.

---

## Overview

Food & Recipe Manager is a console-based systems programming project that demonstrates object-oriented design, separation of concerns, and binary file persistence.

The application allows users to create, manage, and persist ingredients and recipes through a command-driven interface.

---

## Features

- Create and manage ingredients
- Create and manage recipes
- Associate ingredients with recipes
- Persistent storage using binary files
- Layered architecture (Models, Services, Commands)
- Command-based console interaction

---

## Architecture

The project follows a structured multi-layer design.

### Models

Contains the core domain entities:

- `Ingredient`
- `Recipe`
- `RecipeDatabase`
- `RecipeIngredients`

These classes define the data structures used throughout the application.

### Services

Handles business logic:

- `IngredientService`
- `RecipeService`

Services manage operations such as adding, retrieving, and persisting data.

### Commands

Implements user interaction logic:

- `IngredientCommands`
- `RecipeCommands`

This separates presentation logic from business logic.

### Persistence

Data is stored using binary files:

- `ingredients.bin`
- `recipes.bin`

This ensures information persists between application runs.

---

## Technologies Used

- C#
- .NET 8
- Binary file serialization
- Object-Oriented Programming principles

---

## Purpose

This project was developed as part of a Systems Programming portfolio to demonstrate structured application design, persistence mechanisms, and clean separation of responsibilities within a C# console application.
