wor# 🕯️ Lighting Guide: True Darkness & Powerful Flashlight

To make your horror game scary, you need **Pitch Black** darkness and a **Strong** flashlight. Here is how to fix the "White Room" and "Weak Light" issues.

---

### Phase 1: Making the Room Pitch Black (Total Darkness)

Unity by default has a "Sun" and "Sky Light" that lights up everything, even inside closed rooms. We need to kill that light.

#### Step 1: Remove the Sun
1.  In **Hierarchy**, find **Directional Light**.
2.  Click firmly on it and press **Delete**. (Or uncheck the box in Inspector to disable it).

#### Step 2: Kill the Ambient Light (The "Grey" Light)
1.  Go to the Top Menu Bar: **Window** -> **Rendering** -> **Lighting**.
2.  A new window will pop up. Click the **Environment** tab at the top.
3.  Look for **Environment Lighting**:
    *   **Source:** Change it from `Skybox` to **Color**.
    *   **Ambient Color:** Click the color box and make it **Pitch Black** (R:0, G:0, B:0).
4.  **Result:** Your Game View should now be completely black. If you can't see anything, you succeeded!

---

### Phase 2: Making the Flashlight Powerful

Now that the world is dark, your flashlight needs to be strong to cut through it.

#### Step 1: Select the Flashlight
1.  Select your **Spot Light** (Flashlight) in the Hierarchy (inside Main Camera).

#### Step 2: Adjust Settings in Inspector
Change these values to make it look like a real tactical flashlight:

*   **Range:** Increase to `30` or `50` (How far the light goes).
    *   *If this is too low, the light stops halfway.*
*   **Spot Angle:** Set between `40` to `60` (width of the beam).
*   **Intensity:** This is the brightness.
    *   If using **Default/Built-in Render Pipeline**: Set to `2` to `5`.
    *   If using **URP (Universal Render Pipeline)**: You might need to set this to `3000` or higher! Try dragging the slider up until it looks bright.
*   **Color:** Slight yellow or cold blue looks better than pure white. try a very pale yellow (`#FFFAE6`).

#### Step 3: Hard Shadows (Realism)
*   **Shadow Type:** Change from `No Shadows` to **Hard Shadows**.
*   *Why?* This makes objects block the light, creating scary shadows behind them.

---

### Troubleshooting: "Too Bright Up Close" (White Blowout)

If objects become pure white when you get close, your light is too intense or needs a "Cookie".

#### Solution 1: Range vs Intensity Balance
*   **Lower Intensity:** Reduce intensity slightly.
*   **Increase Range:** Keep range high (50+). This allows light to travel far without being blindingly bright at the source.

#### Solution 2: The "Cookie" Texture (Pro Trick) 🍪
A "Cookie" is a mask that filters the light, making it look like a real flashlight with imperfections, rather than a perfect laser circle.
1.  Find any **Flashlight Cookie Texture** online (a black and white image of a flashlight beam).
    *   *Or just search Unity's Default Assets for "Soft" or "Spot".*
2.  In your Spot Light inspector, drag this texture into the **Cookie** slot.
3.  This breaks up the solid white light and makes it look much more realistic and less blinding.

#### Solution 3: Render Mode (Important!)
*   Change **Render Mode** from `Auto` to **Important**. This ensures the light doesn't flicker or disappear when you get close.

#### Solution 4: The Physics Trick (Move It Back) ⬅️
This is usually the **Real Culprit**. 
If your light is exactly at the camera position (0,0,0), when you touch a wall, the distance becomes 0. In physics, `Brightness = 1 / Distance`. If distance is 0, brightness is INFINITE (White Blowout).

**Fix:**
1.  Select your **Spot Light**.
2.  In the Transform component, set **Position Z** to `-0.5` or `-1`.
3.  This moves the light *behind* the player's head. Now even if you kiss the wall, the light is still 0.5 meters away, so it won't be blindingly bright!

---

### Summary Checklist
- [ ] Directional Light (Sun) is Deleted/Disabled.
- [ ] Environment Lighting Source is **Color** (Black).
- [ ] Flashlight Range is high (50+).
- [ ] Flashlight Intensity is high enough to see.
- [ ] Fog is turned OFF.
