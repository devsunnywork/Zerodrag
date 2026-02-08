# 🧠 Smart Flashlight Logic: No More White Blowout

Student, ye script ek "Robotic Arm" ki tarah kaam karti hai. Jaise hi aap deewar ke paas jate hain, ye arm light ko piche khinch leti hai.

---

### The Logic (Step-by-Step)

#### 1. Raycast (Distance Sensor)
`if (Physics.Raycast(ray, out hit, maxDistance))`
*   Code hamesha check karta hai: "Kya 1 meter ke andar koi deewar hai?"

#### 2. The Math (Ratio Calculation)
`float distanceRatio = hit.distance / maxDistance;`
*   Agar deewar **bahut paas** hai (0.1m), toh Ratio kam hoga.
*   Agar deewar **door** hai (0.9m), toh Ratio zyada hoga.

#### 3. Push Back (Target Z)
`targetZ = originalPosition.z - (moveBackAmount * (1 - distanceRatio));`
*   Hum light ko Z-axis par piche bhejte hain.
*   Jitni paas deewar, utna zyada piche light jayegi (Maximum 0.5m piche).

#### 4. Smooth Move (Lerp)
`Mathf.Lerp(current, target, speed)`
*   Ye function ensure karta hai ki light jhatke se piche na jaye, balki makkhan ki tarah smoothly slide kare.

---

### 🛠️ Unity Setup:
1.  **Refill References:**
    *   Kyuki humne script badli hai, ho sakta hai purane links toot gaye hon.
    *   **Light Source:** Apni Spot Light wapas daalein.
    *   **Player Camera:** Apni Main Camera wapas daalein.
2.  **Settings:**
    *   `Max Distance`: **1** (1 meter tak check karega).
    *   `Move Back Amount`: **0.5** (Adha meter piche jayega).
    *   `Smooth Speed`: **10** (Fast reaction).

Ab jab aap deewar ke paas jayenge, dhyan se dekhiyega—Flashlight apne aap piche hat jayegi aur "White Spot" nahi banega! 🔦
