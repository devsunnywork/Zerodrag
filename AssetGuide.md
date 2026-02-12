# Zero Drag - Asset Import & Setup Guide

## 🎯 Goal
Transform your prototype from cubes to a professional sci-fi shooter with consistent art style.

---

## 📦 Phase 1: Download Required Assets

### 1. Robot Models & Props (PRIORITY)
**Source:** Quaternius - Ultimate Sci-Fi Pack  
**Link:** https://quaternius.com/packs/ultimatescifi.html

**What to download:**
- Main pack ZIP file (free, no account needed)

**What you'll use:**
- `Robot_Enemy.fbx` - Your Zero Drag units
- `Robot_Old.fbx` - Player model (Model S-01)
- `Crate_Metal.fbx` - Warehouse props
- `Door_Automatic.fbx` - Exit doors
- `Light_Hanging.fbx` - Warehouse lighting

---

### 2. Weapon Models
**Source:** Kenney Assets  
**Link:** https://kenney.nl/assets/blaster-kit

**What to download:**
- "Blaster Kit" (futuristic guns)
- Download as FBX or OBJ

**What you'll use:**
- `blasterA.fbx` - Player gun
- `blasterB.fbx` - Alternative weapon

---

### 3. Particle Effects (VFX)
**Source:** Unity Asset Store (FREE)  
**Link:** https://assetstore.unity.com/packages/vfx/particles/cartoon-fx-free-109565

**What to download:**
- "Cartoon FX Free" by Jean Moreno

**What you'll use:**
- `CFXM_MuzzleFlash` - Gun fire effect
- `CFXM_Hit_SmokePuff` - Bullet impact
- `CFXM_Explosion` - Enemy destruction (upgrade from fragments)

---

### 4. Environment Textures
**Source:** Polyhaven  
**Link:** https://polyhaven.com/textures

**Search and download:**
- "Metal Floor" (2K resolution, PNG)
- "Concrete Wall" (2K resolution)
- "Rusty Metal" (2K for props)

---

### 5. Audio (Optional for now)
**Source:** Freesound.org  
**Link:** https://freesound.org/

**Search terms:**
- "sci-fi gun shot"
- "robot footsteps"
- "warehouse ambience"

---

## 🔧 Phase 2: Import into Unity

### Step 1: Create Asset Folders
In Unity Project panel:
```
Assets/
├── Models/
│   ├── Characters/
│   ├── Props/
│   └── Weapons/
├── Materials/
├── Prefabs/
├── VFX/
└── Audio/
```

### Step 2: Import Quaternius Pack
1. Extract the ZIP file to your desktop
2. In Unity: Right-click `Assets/Models` → Import New Asset
3. Navigate to extracted folder → Select all .fbx files
4. Click Import

**Post-Import Settings:**
- Select any robot model in Project panel
- Inspector → Rig tab → Animation Type: "Humanoid"
- Click Apply

### Step 3: Import Kenney Weapons
1. Drag .fbx files into `Assets/Models/Weapons/`
2. Select gun model
3. Inspector → Model tab → Scale Factor: 0.1 (Kenney models are large)
4. Click Apply

### Step 4: Import Particle Effects
1. Download from Asset Store (downloads automatically to project)
2. Move prefabs from download folder to `Assets/VFX/`

---

## 🎨 Phase 3: Setup Materials

### Create Robot Material (Metallic Look)
1. Right-click in `Assets/Materials/` → Create → Material
2. Name: "MAT_ZeroDragRobot"
3. Inspector settings:
   - Shader: Standard
   - Metallic: 0.8
   - Smoothness: 0.6
   - Albedo: Dark gray (#2A2A2A)
   - Emission: Enable, Color: Cyan (#00FFFF), Intensity: 0.3

### Create Old Robot Material (Player)
1. Create Material: "MAT_PlayerRobot"
2. Settings:
   - Metallic: 0.5
   - Smoothness: 0.3
   - Albedo: Rusty brown (#5A4A3A)
   - No Emission (looks old/damaged)

---

## 🤖 Phase 4: Replace Cube Enemies

### Step 1: Create Enemy Prefab
1. Drag `Robot_Enemy.fbx` into scene
2. Add Components:
   - Box Collider (adjust to fit model)
   - Rigidbody (Freeze Rotation X, Z)
   - Your existing `EnemyHealth.cs` script
   - Tag: "Enemy"

3. Apply material: Drag `MAT_ZeroDragRobot` onto robot

4. Add glowing eyes:
   - Create → 3D Object → Sphere (tiny, scale 0.1)
   - Position as left eye
   - Material: Create new with Emission (red)
   - Duplicate for right eye

5. Save as Prefab:
   - Drag from Hierarchy to `Assets/Prefabs/`
   - Name: "Enemy_ZeroDragUnit"

### Step 2: Replace Existing Cubes
1. Delete all cube enemies in scene
2. Drag `Enemy_ZeroDragUnit` prefab into scene
3. Duplicate (Ctrl+D) to create multiple enemies
4. Position around warehouse

---

## 🔫 Phase 5: Add Gun to Player

### Step 1: Parent Gun to Camera
1. In Hierarchy: Select "Main Camera"
2. Right-click → 3D Object → Empty (Create child)
3. Name: "GunHolder"
4. Position: X: 0.2, Y: -0.15, Z: 0.4 (bottom-right of screen)

### Step 2: Add Gun Model
1. Drag `blasterA.fbx` into "GunHolder"
2. Rotate to face forward: X: 0, Y: 90, Z: 0
3. Scale: Adjust until it looks right on screen

### Step 3: Add Muzzle Flash Point
1. Right-click GunHolder → Create Empty
2. Name: "MuzzleFlashPoint"
3. Position at gun barrel tip

---

## 🎬 Phase 6: Update Shooting Script

### Modify GunShot.cs
Add this variable at top:
```csharp
public Transform muzzleFlashPoint;
public GameObject muzzleFlashEffect;
```

In `Shoot()` function, add after raycast:
```csharp
// Spawn muzzle flash
GameObject flash = Instantiate(muzzleFlashEffect, muzzleFlashPoint.position, muzzleFlashPoint.rotation);
Destroy(flash, 0.1f);
```

### Setup in Unity
1. Select GunHolder → Inspector
2. Drag `MuzzleFlashPoint` into script field
3. Drag Cartoon FX muzzle flash prefab into effect field

---

## ✅ Verification Checklist

- [ ] Quaternius pack imported
- [ ] Robot materials created (metallic + glowing)
- [ ] Enemy prefab created with proper colliders
- [ ] Cubes replaced with robot models
- [ ] Gun model visible on screen
- [ ] Muzzle flash spawns when shooting
- [ ] Impact particles still work on hit

---

## 🚀 Next Steps After Assets

1. **Lighting:** Add post-processing for bloom (makes glow effects pop)
2. **Animations:** Import walk/idle animations from Mixamo
3. **Environment:** Build warehouse using modular pieces
4. **UI:** Replace text with futuristic HUD

---

**Estimated Time:** 2-3 hours for complete asset integration  
**Difficulty:** Beginner-friendly (mostly drag-and-drop)

Ready to start? Begin with **Phase 1** downloads! 🎮🔥
