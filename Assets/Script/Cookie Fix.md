# 🍪 Quick Fix: Using Unity's Default "Knob" as Cookie

If you don't have a downloaded texture, Unity has a secret hidden texture called **"Knob"** or **"UISprite"** that works great as a simple cookie to soften the light.

### Steps:

1.  **Select Light:** Click your **Spot Light** (Flashlight).
2.  **Find Cookie Slot:** Look for the **Cookie** field in the Inspector.
3.  **Search:** Click the small circle (selector) next to the Cookie slot.
4.  **Type:** Search for `Knob` or `Soft`.
    *   *You should see a soft, blurry white circle.*
5.  **Select:** Double-click it.

### Why this works?
The "Knob" texture is soft at the edges and solid in the center. When used as a cookie, it makes the light fade out smoothly instead of having a hard, sharp edge. This reduces the "White Blowout" effect significantly!

### Still too bright? (Even at 10?) 
If intensity is low but it's STILL white, your **Range** is too small.

**Why?**
In Unity, light falls off over distance. If Range is small (e.g., 10), the light tries to cram all its energy into that short distance, making the start point super bright.

**Fix:**
1.  **Increase Range to 100.** (Yes, really high!)
2.  Now that the light has space to travel, the start point will be much softer.
3.  *Then* adjust intensity. You might even need to increase it back to 20 or 30 now.

### Remove the "Knob" Artifact
If you see the shape of the knob on the wall:
*   Use a **Blurred** cookie texture instead.
*   Or slightly increase **Spot Angle** to blur the edges.
