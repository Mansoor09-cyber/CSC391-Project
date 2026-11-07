using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StarrootRelicSceneBuilder
{
    private static Material mStone;
    private static Material mWood;
    private static Material mGrass;
    private static Material mPath;
    private static Material mAri;
    private static Material mMagicBlue;
    private static Material mGold;
    private static Material mRed;
    private static Material mGreen;
    private static Material mCrystalEmerald;
    private static Material mCrystalSapphire;
    private static Material mCrystalAmethyst;
    private static Material mRuneGlow;
    private static Material mRelicGold;
    private static Material mMossStone;
    private static Material mRuinedWood;
    private static Material mSpikesIron;

    private static List<GameObject> treePrefabs = new List<GameObject>();

    [MenuItem("The Starroot Relic/Build Complete Game Scene")]
    public static void BuildScene()
    {
        Debug.Log("=== STARTING THE STARROOT RELIC SCENE ENHANCEMENT ===");

        // Open existing scene
        var scene = EditorSceneManager.OpenScene("Assets/game.unity");
        if (!scene.IsValid())
        {
            Debug.LogError("Could not open Assets/game.unity!");
            return;
        }

        LoadAssets();

        // 1. Setup Audio Manager
        SetupAudioManager();

        // 2. Enhance Ari
        EnhanceAri();

        // 3. Build dense jungle foliage & borders
        BuildJungleEnvironment();

        // 4. Build Abandoned Ancient Village (5 Destroyed Houses)
        StarrootCrystal crystal1;
        BuildAbandonedVillage(out crystal1);

        // 5. Build Ancient Ruins & Statues
        StarrootCrystal crystal2;
        ClueStele clueStele;
        BuildAncientRuins(out crystal2, out clueStele);

        // 6. Build Interactive Obstacles
        BuildInteractiveObstacles();

        // 7. Build Puzzles (Pressure Plates, Symbols, Crystals)
        AncientGate gateRuin;
        AncientGate gateGrove;
        StarrootCrystal crystal3;
        BuildPuzzles(out gateRuin, out gateGrove, out crystal3);

        // 8. Build Final Starroot Temple & Relic
        AncientGate templeGate;
        StarrootRelic relic;
        BuildStarrootTemple(out templeGate, out relic);

        // 9. Enhance HUD & UI
        GameHUD hud = EnhanceUI();

        // 10. Update GameManager
        UpdateGameManager(hud, templeGate);

        // 11. Environment Lighting & Atmosphere
        SetupAtmosphere();

        // Save scene
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("=== SCENE ENHANCEMENT COMPLETED & SAVED TO Assets/game.unity ===");
    }

    private static void LoadAssets()
    {
        mStone = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/M_Stone.mat");
        mWood = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/M_Wood.mat");
        mGrass = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/M_Grass.mat");
        mPath = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/M_Path.mat");
        mAri = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/M_Ari.mat");
        mMagicBlue = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/M_MagicBlue.mat");
        mGold = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/gold.mat");
        mRed = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/RedMaterial.mat");
        mGreen = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/GreenMaterial.mat");

        mCrystalEmerald = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/M_Crystal_Emerald.mat");
        mCrystalSapphire = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/M_Crystal_Sapphire.mat");
        mCrystalAmethyst = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/M_Crystal_Amethyst.mat");
        mRuneGlow = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/M_RuneGlow.mat");
        mRelicGold = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/M_RelicGold.mat");
        mMossStone = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/M_MossStone.mat");
        mRuinedWood = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/M_RuinedWood.mat");
        mSpikesIron = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/M_SpikesIron.mat");

        treePrefabs.Clear();
        string[] treeGuids = AssetDatabase.FindAssets("t:Prefab", new string[] { "Assets/BrokenVector/LowPolyTreePack/Prefabs" });
        foreach (var guid in treeGuids)
        {
            string p = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(p);
            if (prefab != null) treePrefabs.Add(prefab);
        }
        Debug.Log($"Loaded {treePrefabs.Count} tree prefabs from LowPolyTreePack.");
    }

    private static void SetupAudioManager()
    {
        GameObject audioObj = GameObject.Find("ProceduralAudioManager");
        if (audioObj == null)
        {
            audioObj = new GameObject("ProceduralAudioManager");
            audioObj.AddComponent<ProceduralAudioManager>();
        }
    }

    private static void EnhanceAri()
    {
        GameObject ari = GameObject.Find("Ari");
        if (ari == null)
        {
            Debug.LogWarning("Existing Ari not found!");
            return;
        }

        // Hide root capsule mesh renderer if present
        MeshRenderer rootRenderer = ari.GetComponent<MeshRenderer>();
        if (rootRenderer != null)
        {
            rootRenderer.enabled = false;
        }

        // Remove old 'Nose' child if present
        Transform oldNose = ari.transform.Find("Nose");
        if (oldNose != null)
        {
            Object.DestroyImmediate(oldNose.gameObject);
        }

        // Check if AriVisuals already exists
        Transform visuals = ari.transform.Find("AriVisuals");
        if (visuals != null)
        {
            Object.DestroyImmediate(visuals.gameObject);
        }

        GameObject visualRoot = new GameObject("AriVisuals");
        visualRoot.transform.SetParent(ari.transform, false);
        visualRoot.transform.localPosition = Vector3.zero;

        // Torso / Explorer Jacket
        GameObject torso = CreatePrimitive(PrimitiveType.Cube, "Torso", visualRoot.transform,
            new Vector3(0f, 1.25f, 0f), new Vector3(0.46f, 0.54f, 0.30f), mAri);

        // Explorer Belt
        CreatePrimitive(PrimitiveType.Cube, "Belt", torso.transform,
            new Vector3(0f, -0.22f, 0f), new Vector3(1.06f, 0.20f, 1.08f), mRuinedWood != null ? mRuinedWood : mWood);

        // Canteen
        CreatePrimitive(PrimitiveType.Cylinder, "Canteen", torso.transform,
            new Vector3(0.56f, -0.18f, 0.1f), new Vector3(0.24f, 0.18f, 0.18f), mStone);

        // Expedition Pouch
        CreatePrimitive(PrimitiveType.Cube, "Pouch", torso.transform,
            new Vector3(-0.56f, -0.18f, 0.1f), new Vector3(0.22f, 0.24f, 0.20f), mWood);

        // Expedition Backpack
        GameObject backpack = CreatePrimitive(PrimitiveType.Cube, "Backpack", torso.transform,
            new Vector3(0f, 0.05f, -0.66f), new Vector3(0.78f, 0.75f, 0.44f), mRuinedWood != null ? mRuinedWood : mWood);

        // Rolled Sleeping Bag on top of backpack
        GameObject bedroll = CreatePrimitive(PrimitiveType.Cylinder, "Bedroll", backpack.transform,
            new Vector3(0f, 0.58f, 0f), new Vector3(0.35f, 0.65f, 0.35f), mGrass);
        bedroll.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);

        // Head
        GameObject head = CreatePrimitive(PrimitiveType.Cube, "Head", visualRoot.transform,
            new Vector3(0f, 1.68f, 0f), new Vector3(0.28f, 0.30f, 0.28f), mAri);

        // Adventurer Hair / Ponytail
        CreatePrimitive(PrimitiveType.Cube, "Hair_Top", head.transform,
            new Vector3(0f, 0.52f, -0.1f), new Vector3(1.08f, 0.45f, 1.15f), mWood);

        GameObject ponytail = CreatePrimitive(PrimitiveType.Cylinder, "Ponytail", head.transform,
            new Vector3(0f, 0.32f, -0.7f), new Vector3(0.32f, 0.55f, 0.32f), mWood);
        ponytail.transform.localRotation = Quaternion.Euler(25f, 0f, 0f);

        // Explorer Bandana
        CreatePrimitive(PrimitiveType.Cube, "Bandana", head.transform,
            new Vector3(0f, 0.15f, 0f), new Vector3(1.04f, 0.18f, 1.04f), mRed);

        // Left Arm
        GameObject leftArmPivot = new GameObject("LeftArm_Pivot");
        leftArmPivot.transform.SetParent(visualRoot.transform, false);
        leftArmPivot.transform.localPosition = new Vector3(-0.32f, 1.45f, 0f);
        GameObject leftArmMesh = CreatePrimitive(PrimitiveType.Cube, "LeftArm_Mesh", leftArmPivot.transform,
            new Vector3(0f, -0.3f, 0f), new Vector3(0.15f, 0.62f, 0.15f), mAri);

        // Right Arm
        GameObject rightArmPivot = new GameObject("RightArm_Pivot");
        rightArmPivot.transform.SetParent(visualRoot.transform, false);
        rightArmPivot.transform.localPosition = new Vector3(0.32f, 1.45f, 0f);
        GameObject rightArmMesh = CreatePrimitive(PrimitiveType.Cube, "RightArm_Mesh", rightArmPivot.transform,
            new Vector3(0f, -0.3f, 0f), new Vector3(0.15f, 0.62f, 0.15f), mAri);

        // Left Leg
        GameObject leftLegPivot = new GameObject("LeftLeg_Pivot");
        leftLegPivot.transform.SetParent(visualRoot.transform, false);
        leftLegPivot.transform.localPosition = new Vector3(-0.14f, 0.95f, 0f);
        GameObject leftLegMesh = CreatePrimitive(PrimitiveType.Cube, "LeftLeg_Mesh", leftLegPivot.transform,
            new Vector3(0f, -0.42f, 0f), new Vector3(0.18f, 0.85f, 0.18f), mPath);
        CreatePrimitive(PrimitiveType.Cube, "LeftBoot", leftLegPivot.transform,
            new Vector3(0f, -0.88f, 0.06f), new Vector3(0.20f, 0.18f, 0.30f), mStone);

        // Right Leg
        GameObject rightLegPivot = new GameObject("RightLeg_Pivot");
        rightLegPivot.transform.SetParent(visualRoot.transform, false);
        rightLegPivot.transform.localPosition = new Vector3(0.14f, 0.95f, 0f);
        GameObject rightLegMesh = CreatePrimitive(PrimitiveType.Cube, "RightLeg_Mesh", rightLegPivot.transform,
            new Vector3(0f, -0.42f, 0f), new Vector3(0.18f, 0.85f, 0.18f), mPath);
        CreatePrimitive(PrimitiveType.Cube, "RightBoot", rightLegPivot.transform,
            new Vector3(0f, -0.88f, 0.06f), new Vector3(0.20f, 0.18f, 0.30f), mStone);

        // Attach Procedural Animator
        AriProceduralAnimator animator = ari.GetComponent<AriProceduralAnimator>();
        if (animator == null) animator = ari.AddComponent<AriProceduralAnimator>();
        animator.torso = torso.transform;
        animator.head = head.transform;
        animator.backpack = backpack.transform;
        animator.leftArm = leftArmPivot.transform;
        animator.rightArm = rightArmPivot.transform;
        animator.leftLeg = leftLegPivot.transform;
        animator.rightLeg = rightLegPivot.transform;

        // Attach PlayerInteraction
        if (ari.GetComponent<PlayerInteraction>() == null)
        {
            ari.AddComponent<PlayerInteraction>();
        }

        // Configure PlayerMover
        PlayerMover mover = ari.GetComponent<PlayerMover>();
        if (mover != null)
        {
            mover.speed = 6.0f;
            mover.sprintSpeed = 9.5f;
            mover.jumpHeight = 1.6f;
            mover.gravity = -20f;
            if (Camera.main != null) mover.cameraTransform = Camera.main.transform;
        }

        Debug.Log("Ari character enhanced with female adventurer model and procedural animator.");
    }

    private static void BuildJungleEnvironment()
    {
        GameObject envParent = GameObject.Find("DenseJungleEnvironment");
        if (envParent != null) Object.DestroyImmediate(envParent);
        envParent = new GameObject("DenseJungleEnvironment");

        if (treePrefabs.Count == 0) return;

        // Place perimeter forest border
        // South border (Z: -16 to -13, X: -15 to 35)
        for (float x = -15f; x <= 35f; x += 3.2f)
        {
            PlaceTree(envParent.transform, new Vector3(x, 0f, -14f + Random.Range(-1.5f, 1.5f)), Random.Range(1.0f, 1.5f));
            PlaceTree(envParent.transform, new Vector3(x, 0f, -17f + Random.Range(-1.5f, 1.5f)), Random.Range(1.2f, 1.8f));
        }

        // West border (X: -18 to -14, Z: -15 to 45)
        for (float z = -14f; z <= 45f; z += 3.5f)
        {
            PlaceTree(envParent.transform, new Vector3(-16f + Random.Range(-1.5f, 1.5f), 0f, z), Random.Range(1.1f, 1.6f));
            PlaceTree(envParent.transform, new Vector3(-19f + Random.Range(-1.5f, 1.5f), 0f, z), Random.Range(1.3f, 1.9f));
        }

        // North border (Z: 48 to 55, X: -15 to 35)
        for (float x = -15f; x <= 35f; x += 3.2f)
        {
            PlaceTree(envParent.transform, new Vector3(x, 0f, 52f + Random.Range(-1.5f, 1.5f)), Random.Range(1.1f, 1.7f));
        }

        // East border (X: 32 to 38, Z: -15 to 25)
        for (float z = -14f; z <= 25f; z += 3.5f)
        {
            PlaceTree(envParent.transform, new Vector3(34f + Random.Range(-1.5f, 1.5f), 0f, z), Random.Range(1.1f, 1.6f));
        }

        // Natural jungle grove trees flanking main pathway (keeping clear path along X: 8 to 12)
        PlaceTree(envParent.transform, new Vector3(5f, 0f, -5f), 1.2f);
        PlaceTree(envParent.transform, new Vector3(15f, 0f, -6f), 1.3f);
        PlaceTree(envParent.transform, new Vector3(4f, 0f, 1f), 1.4f);
        PlaceTree(envParent.transform, new Vector3(16f, 0f, 2f), 1.1f);
        PlaceTree(envParent.transform, new Vector3(5f, 0f, 8f), 1.5f);
        PlaceTree(envParent.transform, new Vector3(15f, 0f, 9f), 1.3f);

        Debug.Log("Dense jungle perimeter and path trees placed.");
    }

    private static void PlaceTree(Transform parent, Vector3 pos, float scale)
    {
        if (treePrefabs.Count == 0) return;
        GameObject prefab = treePrefabs[Random.Range(0, treePrefabs.Count)];
        GameObject tree = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
        tree.transform.position = pos;
        tree.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        tree.transform.localScale = Vector3.one * scale;

        // Ensure collider on trunk so player cannot pass through
        if (tree.GetComponentInChildren<Collider>() == null)
        {
            CapsuleCollider col = tree.AddComponent<CapsuleCollider>();
            col.radius = 0.5f * scale;
            col.height = 6.0f * scale;
            col.center = new Vector3(0f, 3f * scale, 0f);
        }
    }

    private static void BuildAbandonedVillage(out StarrootCrystal crystal1)
    {
        GameObject village = GameObject.Find("AbandonedVillage");
        if (village != null) Object.DestroyImmediate(village);
        village = new GameObject("AbandonedVillage");

        Material wallMat = mMossStone != null ? mMossStone : mStone;
        Material woodMat = mRuinedWood != null ? mRuinedWood : mWood;

        // Village entrance stone archway connecting from bridge
        CreateStoneArch(village.transform, new Vector3(2f, 0f, 18f), 90f, wallMat);

        // House 1: Overgrown Ruined Cottage (-8, 0, 15)
        GameObject h1 = new GameObject("House_01_OvergrownCottage");
        h1.transform.SetParent(village.transform);
        h1.transform.position = new Vector3(-8f, 0f, 15f);
        BuildRuinedHouse(h1.transform, 6f, 5f, 2.8f, wallMat, woodMat, 0.4f);

        // House 2: Shattered Watchtower (-15, 0, 20)
        GameObject h2 = new GameObject("House_02_ShatteredWatchtower");
        h2.transform.SetParent(village.transform);
        h2.transform.position = new Vector3(-15f, 0f, 20f);
        BuildWatchtowerRuin(h2.transform, wallMat, woodMat);

        // House 3: Elder's Ruined Sanctuary (-10, 0, 28) - Hides Crystal 1!
        GameObject h3 = new GameObject("House_03_ElderSanctuary");
        h3.transform.SetParent(village.transform);
        h3.transform.position = new Vector3(-10f, 0f, 28f);
        BuildSanctuaryRuin(h3.transform, wallMat, woodMat);

        // Spawn Crystal #1 (Emerald) in Sanctuary inner alcove
        GameObject c1Obj = CreatePrimitive(PrimitiveType.Sphere, "StarrootCrystal_Emerald", h3.transform,
            new Vector3(0f, 1.2f, 0f), new Vector3(0.55f, 0.75f, 0.55f), mCrystalEmerald);
        crystal1 = c1Obj.AddComponent<StarrootCrystal>();
        crystal1.crystalIndex = 0;
        crystal1.crystalName = "Emerald Starroot Crystal";
        crystal1.crystalColor = new Color(0.2f, 1f, 0.4f);
        crystal1.crystalRenderer = c1Obj.GetComponent<MeshRenderer>();
        Light l1 = c1Obj.AddComponent<Light>();
        l1.type = LightType.Point;
        l1.range = 5f;
        l1.color = crystal1.crystalColor;
        crystal1.crystalLight = l1;
        SphereCollider sc1 = c1Obj.GetComponent<SphereCollider>();
        if (sc1 != null) sc1.isTrigger = true;

        // House 4: Sunken Ancient Dwelling (-16, 0, 30)
        GameObject h4 = new GameObject("House_04_SunkenDwelling");
        h4.transform.SetParent(village.transform);
        h4.transform.position = new Vector3(-16f, 0f, 30f);
        BuildRuinedHouse(h4.transform, 5f, 5f, 2.2f, wallMat, woodMat, 0.6f);

        // House 5: Workshop & Ancient Well Courtyard (-4, 0, 24)
        GameObject h5 = new GameObject("House_05_WorkshopAndWell");
        h5.transform.SetParent(village.transform);
        h5.transform.position = new Vector3(-4f, 0f, 24f);
        BuildWorkshopAndWell(h5.transform, wallMat, woodMat);

        Debug.Log("Abandoned Ancient Village (5 Destroyed Houses) created.");
    }

    private static void BuildRuinedHouse(Transform parent, float width, float length, float height, Material wallM, Material woodM, float collapseRatio)
    {
        // Stone Floor
        CreatePrimitive(PrimitiveType.Cube, "Floor", parent, new Vector3(0f, 0.05f, 0f), new Vector3(width, 0.1f, length), mPath);

        // Left wall (partially standing)
        CreatePrimitive(PrimitiveType.Cube, "Wall_Left", parent,
            new Vector3(-width * 0.5f, height * 0.5f, 0f), new Vector3(0.4f, height, length), wallM);

        // Back wall
        CreatePrimitive(PrimitiveType.Cube, "Wall_Back", parent,
            new Vector3(0f, height * 0.4f, length * 0.5f), new Vector3(width, height * 0.8f, 0.4f), wallM);

        // Right wall (broken section)
        CreatePrimitive(PrimitiveType.Cube, "Wall_Right_A", parent,
            new Vector3(width * 0.5f, height * 0.25f, -length * 0.25f), new Vector3(0.4f, height * 0.5f, length * 0.5f), wallM);

        // Front wall with doorway
        CreatePrimitive(PrimitiveType.Cube, "Wall_Front_Left", parent,
            new Vector3(-width * 0.3f, height * 0.5f, -length * 0.5f), new Vector3(width * 0.4f, height, 0.4f), wallM);
        CreatePrimitive(PrimitiveType.Cube, "Wall_Front_Right", parent,
            new Vector3(width * 0.35f, height * 0.3f, -length * 0.5f), new Vector3(width * 0.3f, height * 0.6f, 0.4f), wallM);

        // Collapsed wooden roof rafters fallen into center
        GameObject rafter1 = CreatePrimitive(PrimitiveType.Cube, "Rafter_Collapsed_1", parent,
            new Vector3(0f, height * 0.5f, 0f), new Vector3(0.2f, 0.2f, length * 0.9f), woodM);
        rafter1.transform.localRotation = Quaternion.Euler(22f, 15f, 18f);

        GameObject rafter2 = CreatePrimitive(PrimitiveType.Cube, "Rafter_Collapsed_2", parent,
            new Vector3(-width * 0.2f, height * 0.4f, length * 0.1f), new Vector3(width * 0.8f, 0.2f, 0.2f), woodM);
        rafter2.transform.localRotation = Quaternion.Euler(-18f, 35f, -25f);
    }

    private static void BuildWatchtowerRuin(Transform parent, Material wallM, Material woodM)
    {
        // Raised stone foundation
        CreatePrimitive(PrimitiveType.Cube, "Foundation", parent, new Vector3(0f, 0.5f, 0f), new Vector3(5f, 1.0f, 5f), wallM);

        // Broken stone stairs
        for (int i = 0; i < 3; i++)
        {
            CreatePrimitive(PrimitiveType.Cube, $"Stair_{i}", parent,
                new Vector3(0f, 0.18f * (i + 1), -3.0f + (i * 0.5f)), new Vector3(2.5f, 0.2f, 0.6f), wallM);
        }

        // Crumbled tower walls
        CreatePrimitive(PrimitiveType.Cube, "Wall_North", parent, new Vector3(0f, 2.5f, 2.2f), new Vector3(4.5f, 3.5f, 0.5f), wallM);
        CreatePrimitive(PrimitiveType.Cube, "Wall_West", parent, new Vector3(-2.2f, 2.0f, 0f), new Vector3(0.5f, 2.8f, 4.5f), wallM);
        CreatePrimitive(PrimitiveType.Cube, "Broken_Corner", parent, new Vector3(2.0f, 1.5f, -1.8f), new Vector3(1.2f, 1.8f, 1.2f), wallM);
    }

    private static void BuildSanctuaryRuin(Transform parent, Material wallM, Material woodM)
    {
        // Grand stone dais
        CreatePrimitive(PrimitiveType.Cube, "SanctuaryDais", parent, new Vector3(0f, 0.2f, 0f), new Vector3(7f, 0.4f, 7f), wallM);

        // Fluted pillars
        CreatePrimitive(PrimitiveType.Cylinder, "Pillar_FL", parent, new Vector3(-2.8f, 1.8f, -2.8f), new Vector3(0.6f, 1.6f, 0.6f), wallM);
        CreatePrimitive(PrimitiveType.Cylinder, "Pillar_FR", parent, new Vector3(2.8f, 1.8f, -2.8f), new Vector3(0.6f, 1.6f, 0.6f), wallM);
        CreatePrimitive(PrimitiveType.Cylinder, "Pillar_BL", parent, new Vector3(-2.8f, 1.2f, 2.8f), new Vector3(0.6f, 1.0f, 0.6f), wallM); // Broken half height

        // Broken arch lintel
        GameObject arch = CreatePrimitive(PrimitiveType.Cube, "ArchLintel", parent, new Vector3(0f, 3.5f, -2.8f), new Vector3(6.2f, 0.5f, 0.6f), wallM);

        // Sanctum inner pedestal
        CreatePrimitive(PrimitiveType.Cylinder, "CrystalPedestal", parent, new Vector3(0f, 0.5f, 0f), new Vector3(0.9f, 0.5f, 0.9f), wallM);
    }

    private static void BuildWorkshopAndWell(Transform parent, Material wallM, Material woodM)
    {
        // Ancient Stone Well
        GameObject well = CreatePrimitive(PrimitiveType.Cylinder, "AncientWell", parent,
            new Vector3(2f, 0.45f, 0f), new Vector3(1.8f, 0.45f, 1.8f), wallM);
        // Well roof crossbeam
        CreatePrimitive(PrimitiveType.Cube, "Well_Post_L", parent, new Vector3(1.1f, 1.1f, 0f), new Vector3(0.15f, 1.4f, 0.15f), woodM);
        CreatePrimitive(PrimitiveType.Cube, "Well_Post_R", parent, new Vector3(2.9f, 1.1f, 0f), new Vector3(0.15f, 1.4f, 0.15f), woodM);
        CreatePrimitive(PrimitiveType.Cube, "Well_Beam", parent, new Vector3(2.0f, 1.8f, 0f), new Vector3(2.0f, 0.15f, 0.2f), woodM);

        // Workshop ruined shelter
        CreatePrimitive(PrimitiveType.Cube, "WorkshopWall", parent, new Vector3(-2.5f, 1.5f, 0f), new Vector3(0.4f, 2.5f, 4f), wallM);
        GameObject roof = CreatePrimitive(PrimitiveType.Cube, "WorkshopRoof", parent, new Vector3(-1.2f, 2.4f, 0f), new Vector3(2.8f, 0.15f, 3.8f), woodM);
        roof.transform.localRotation = Quaternion.Euler(0f, 0f, -16f);
    }

    private static void BuildAncientRuins(out StarrootCrystal crystal2, out ClueStele clueStele)
    {
        GameObject ruins = GameObject.Find("AncientRuins");
        if (ruins != null) Object.DestroyImmediate(ruins);
        ruins = new GameObject("AncientRuins");

        Material wallMat = mMossStone != null ? mMossStone : mStone;

        // Guardian Statue West (-5, 0, 44)
        BuildGuardianStatue(ruins.transform, new Vector3(-5f, 0f, 44f), 0f, wallMat);

        // Guardian Statue East (-1f, 0, 44) - Hides Crystal 2!
        GameObject statueEast = BuildGuardianStatue(ruins.transform, new Vector3(-1f, 0f, 44f), 0f, wallMat);

        // Spawn Crystal #2 (Sapphire) nestled behind East Guardian Statue
        GameObject c2Obj = CreatePrimitive(PrimitiveType.Sphere, "StarrootCrystal_Sapphire", ruins.transform,
            new Vector3(-1f, 1.4f, 46.2f), new Vector3(0.55f, 0.75f, 0.55f), mCrystalSapphire);
        crystal2 = c2Obj.AddComponent<StarrootCrystal>();
        crystal2.crystalIndex = 1;
        crystal2.crystalName = "Sapphire Starroot Crystal";
        crystal2.crystalColor = new Color(0.2f, 0.6f, 1f);
        crystal2.crystalRenderer = c2Obj.GetComponent<MeshRenderer>();
        Light l2 = c2Obj.AddComponent<Light>();
        l2.type = LightType.Point;
        l2.range = 5f;
        l2.color = crystal2.crystalColor;
        crystal2.crystalLight = l2;
        SphereCollider sc2 = c2Obj.GetComponent<SphereCollider>();
        if (sc2 != null) sc2.isTrigger = true;

        // Ancient Inscribed Stele with riddle
        GameObject steleObj = CreatePrimitive(PrimitiveType.Cube, "AncientClueStele", ruins.transform,
            new Vector3(-3f, 1.2f, 42f), new Vector3(0.8f, 2.2f, 0.35f), wallMat);
        clueStele = steleObj.AddComponent<ClueStele>();

        // Monolithic tall pillars
        CreatePrimitive(PrimitiveType.Cylinder, "ColossalPillar_1", ruins.transform, new Vector3(-7f, 3.0f, 40f), new Vector3(0.9f, 3.0f, 0.9f), wallMat);
        CreatePrimitive(PrimitiveType.Cylinder, "ColossalPillar_2", ruins.transform, new Vector3(1f, 3.0f, 40f), new Vector3(0.9f, 3.0f, 0.9f), wallMat);

        // Broken pillar fallen on ground
        GameObject brokenPillar = CreatePrimitive(PrimitiveType.Cylinder, "FallenPillar", ruins.transform,
            new Vector3(-3f, 0.45f, 46f), new Vector3(0.85f, 1.8f, 0.85f), wallMat);
        brokenPillar.transform.localRotation = Quaternion.Euler(85f, 25f, 0f);

        Debug.Log("Ancient Ruins & Guardian Statues created.");
    }

    private static GameObject BuildGuardianStatue(Transform parent, Vector3 pos, float rotY, Material mat)
    {
        GameObject statue = new GameObject("AncientGuardianStatue");
        statue.transform.SetParent(parent);
        statue.transform.position = pos;
        statue.transform.rotation = Quaternion.Euler(0f, rotY, 0f);

        // Plinth
        CreatePrimitive(PrimitiveType.Cube, "Plinth", statue.transform, new Vector3(0f, 0.4f, 0f), new Vector3(1.6f, 0.8f, 1.6f), mat);
        // Body Monolith
        CreatePrimitive(PrimitiveType.Cube, "Body", statue.transform, new Vector3(0f, 2.0f, 0f), new Vector3(1.1f, 2.5f, 0.9f), mat);
        // Head
        CreatePrimitive(PrimitiveType.Cube, "Head", statue.transform, new Vector3(0f, 3.7f, 0f), new Vector3(0.85f, 0.95f, 0.85f), mat);

        // Glowing gem eyes
        CreatePrimitive(PrimitiveType.Sphere, "Eye_L", statue.transform,
            new Vector3(-0.2f, 3.75f, 0.45f), new Vector3(0.12f, 0.12f, 0.12f), mRuneGlow);
        CreatePrimitive(PrimitiveType.Sphere, "Eye_R", statue.transform,
            new Vector3(0.2f, 3.75f, 0.45f), new Vector3(0.12f, 0.12f, 0.12f), mRuneGlow);

        return statue;
    }

    private static void BuildInteractiveObstacles()
    {
        GameObject obsParent = GameObject.Find("InteractiveObstacles");
        if (obsParent != null) Object.DestroyImmediate(obsParent);
        obsParent = new GameObject("InteractiveObstacles");

        Material wallMat = mMossStone != null ? mMossStone : mStone;
        Material woodMat = mRuinedWood != null ? mRuinedWood : mWood;
        Material ironMat = mSpikesIron != null ? mSpikesIron : mStone;

        // Obstacle 1: Falling Rock Trap (at Z: -2, Path X: 10)
        GameObject rockTrapObj = new GameObject("Obstacle_01_FallingRockTrap");
        rockTrapObj.transform.SetParent(obsParent.transform);
        rockTrapObj.transform.position = new Vector3(10f, 0f, -2f);

        BoxCollider rockTrigger = rockTrapObj.AddComponent<BoxCollider>();
        rockTrigger.isTrigger = true;
        rockTrigger.size = new Vector3(5f, 3f, 3f);
        rockTrigger.center = new Vector3(0f, 1.5f, 0f);

        // Cliff ledge
        CreatePrimitive(PrimitiveType.Cube, "CliffLedge", rockTrapObj.transform,
            new Vector3(4.5f, 2.5f, 2.5f), new Vector3(2.5f, 4f, 3f), wallMat);

        // Boulder
        GameObject boulder = CreatePrimitive(PrimitiveType.Sphere, "Boulder", rockTrapObj.transform,
            new Vector3(4.5f, 4.6f, 2.5f), new Vector3(1.6f, 1.6f, 1.6f), wallMat);
        Rigidbody boulderRb = boulder.AddComponent<Rigidbody>();
        boulderRb.mass = 200f;
        boulderRb.isKinematic = true;

        FallingRockTrap rockTrap = rockTrapObj.AddComponent<FallingRockTrap>();
        rockTrap.rockRigidbody = boulderRb;
        rockTrap.initialForce = new Vector3(-8f, 0f, -4f);

        // Obstacle 2: Moving Spiked Log Trap (at Z: 5, Path X: 10)
        GameObject logTrapObj = new GameObject("Obstacle_02_MovingLogTrap");
        logTrapObj.transform.SetParent(obsParent.transform);
        logTrapObj.transform.position = new Vector3(10f, 0.8f, 5f);

        GameObject logMesh = CreatePrimitive(PrimitiveType.Cylinder, "SpikedLog", logTrapObj.transform,
            Vector3.zero, new Vector3(0.55f, 2.2f, 0.55f), woodMat);
        logMesh.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);

        // Wooden spikes on log
        for (int i = -2; i <= 2; i++)
        {
            CreatePrimitive(PrimitiveType.Cylinder, $"Spike_{i}", logMesh.transform,
                new Vector3(0.6f, i * 0.4f, 0f), new Vector3(0.12f, 0.35f, 0.12f), ironMat);
        }

        BoxCollider logCol = logTrapObj.AddComponent<BoxCollider>();
        logCol.isTrigger = true;
        logCol.size = new Vector3(4.5f, 1.2f, 1.2f);

        MovingLogTrap logTrap = logTrapObj.AddComponent<MovingLogTrap>();
        logTrap.moveAxis = new Vector3(1f, 0f, 0f);
        logTrap.distance = 4.2f;
        logTrap.speed = 2.4f;

        // Obstacle 3: Retractable Spike Trap (at Z: 10, Path X: 10)
        GameObject spikeTrapObj = new GameObject("Obstacle_03_SpikeTrap");
        spikeTrapObj.transform.SetParent(obsParent.transform);
        spikeTrapObj.transform.position = new Vector3(10f, 0.05f, 10f);

        // Stone frame grate
        CreatePrimitive(PrimitiveType.Cube, "SpikeFrame", spikeTrapObj.transform,
            Vector3.zero, new Vector3(3.2f, 0.1f, 3.2f), wallMat);

        GameObject spikesGroup = new GameObject("SpikesGroup");
        spikesGroup.transform.SetParent(spikeTrapObj.transform, false);

        for (float sx = -1.0f; sx <= 1.0f; sx += 0.5f)
        {
            for (float sz = -1.0f; sz <= 1.0f; sz += 0.5f)
            {
                CreatePrimitive(PrimitiveType.Cylinder, "Spike", spikesGroup.transform,
                    new Vector3(sx, 0.25f, sz), new Vector3(0.08f, 0.35f, 0.08f), ironMat);
            }
        }

        BoxCollider spikeCol = spikeTrapObj.AddComponent<BoxCollider>();
        spikeCol.isTrigger = true;
        spikeCol.size = new Vector3(2.8f, 1.2f, 2.8f);
        spikeCol.center = new Vector3(0f, 0.6f, 0f);

        SpikeTrap spikeTrap = spikeTrapObj.AddComponent<SpikeTrap>();
        spikeTrap.spikesMeshTransform = spikesGroup.transform;
        spikeTrap.loweredY = -0.3f;
        spikeTrap.raisedY = 0.25f;

        // Obstacle 4: Collapsing Wooden Bridge (at Z: 15 to 19, Path X: 10)
        GameObject bridgeObj = new GameObject("Obstacle_04_CollapsingBridge");
        bridgeObj.transform.SetParent(obsParent.transform);
        bridgeObj.transform.position = new Vector3(10f, 0.15f, 17f);

        // Ravine markers under bridge
        CreatePrimitive(PrimitiveType.Cube, "Ravine_L", bridgeObj.transform, new Vector3(-2.2f, -1.2f, 0f), new Vector3(1.2f, 2.4f, 5.5f), wallMat);
        CreatePrimitive(PrimitiveType.Cube, "Ravine_R", bridgeObj.transform, new Vector3(2.2f, -1.2f, 0f), new Vector3(1.2f, 2.4f, 5.5f), wallMat);

        // 4 Planks
        for (int i = 0; i < 4; i++)
        {
            float pz = -1.8f + (i * 1.2f);
            GameObject plank = CreatePrimitive(PrimitiveType.Cube, $"BridgePlank_{i}", bridgeObj.transform,
                new Vector3(0f, 0f, pz), new Vector3(3.2f, 0.12f, 0.95f), woodMat);
            BoxCollider pCol = plank.GetComponent<BoxCollider>();
            pCol.isTrigger = false;

            // Trigger overlay for collapse detection
            GameObject trigObj = new GameObject("PlankTrigger");
            trigObj.transform.SetParent(plank.transform, false);
            BoxCollider tc = trigObj.AddComponent<BoxCollider>();
            tc.isTrigger = true;
            tc.size = new Vector3(1.0f, 3.0f, 1.0f);
            tc.center = new Vector3(0f, 1.5f, 0f);

            plank.AddComponent<CollapsingBridge>();
        }

        Debug.Log("4 Functional Obstacles created along Zone 2.");
    }

    private static void BuildPuzzles(out AncientGate gateRuin, out AncientGate gateGrove, out StarrootCrystal crystal3)
    {
        GameObject puzzleParent = GameObject.Find("AncientPuzzleArea");
        if (puzzleParent != null) Object.DestroyImmediate(puzzleParent);
        puzzleParent = new GameObject("AncientPuzzleArea");

        Material wallMat = mMossStone != null ? mMossStone : mStone;

        // Intermediate Gate 1 (Ruin Gate) at (15, 0, 40)
        gateRuin = BuildAncientGate(puzzleParent.transform, new Vector3(15f, 0f, 40f), 90f, "Gate_PressurePlates", wallMat);

        // Puzzle 1: 3 Pressure Plates
        GameObject ppPuzzleObj = new GameObject("Puzzle_01_PressurePlates");
        ppPuzzleObj.transform.SetParent(puzzleParent.transform);
        ppPuzzleObj.transform.position = new Vector3(10f, 0f, 40f);

        PressurePlatePuzzle ppp = ppPuzzleObj.AddComponent<PressurePlatePuzzle>();
        ppp.linkedGate = gateRuin;
        ppp.targetSequence = new int[] { 0, 1, 2 };

        PressurePlate[] plates = new PressurePlate[3];
        Vector3[] plateOffsets = new Vector3[] {
            new Vector3(-3f, 0.05f, -2f),
            new Vector3(0f, 0.05f, 1.5f),
            new Vector3(3f, 0.05f, -2f)
        };

        for (int i = 0; i < 3; i++)
        {
            GameObject pObj = new GameObject($"PressurePlate_{i + 1}");
            pObj.transform.SetParent(ppPuzzleObj.transform);
            pObj.transform.position = ppPuzzleObj.transform.position + plateOffsets[i];

            // Base rim
            CreatePrimitive(PrimitiveType.Cylinder, "BaseRim", pObj.transform, Vector3.zero, new Vector3(1.6f, 0.08f, 1.6f), wallMat);

            // Steppable plate
            GameObject stepMesh = CreatePrimitive(PrimitiveType.Cylinder, "PlateMesh", pObj.transform,
                new Vector3(0f, 0.06f, 0f), new Vector3(1.4f, 0.08f, 1.4f), mRuneGlow != null ? mRuneGlow : mGold);

            BoxCollider plateCol = pObj.AddComponent<BoxCollider>();
            plateCol.isTrigger = true;
            plateCol.size = new Vector3(1.5f, 0.5f, 1.5f);
            plateCol.center = new Vector3(0f, 0.25f, 0f);

            PressurePlate pp = pObj.AddComponent<PressurePlate>();
            pp.plateIndex = i;
            pp.puzzleController = ppp;
            pp.plateMeshTransform = stepMesh.transform;
            Light pl = pObj.AddComponent<Light>();
            pl.type = LightType.Point;
            pl.range = 3.5f;
            pl.color = Color.yellow;
            pp.runeLight = pl;

            plates[i] = pp;
        }
        ppp.plates = plates;

        // Intermediate Gate 2 (Grove Gate) at (22, 0, 46)
        gateGrove = BuildAncientGate(puzzleParent.transform, new Vector3(22f, 0f, 46f), 0f, "Gate_Symbols", wallMat);

        // Puzzle 2: 4 Ancient Symbols (Sun, Star, Moon, Serpent)
        GameObject symPuzzleObj = new GameObject("Puzzle_02_AncientSymbols");
        symPuzzleObj.transform.SetParent(puzzleParent.transform);
        symPuzzleObj.transform.position = new Vector3(18f, 0f, 46f);

        SymbolPuzzle sp = symPuzzleObj.AddComponent<SymbolPuzzle>();
        sp.linkedGate = gateGrove;
        sp.targetSequence = new int[] { 0, 1, 2, 3 };

        string[] symNames = new string[] { "Sun", "Star", "Moon", "Serpent" };
        Vector3[] symOffsets = new Vector3[] {
            new Vector3(-2.5f, 0f, -3f),
            new Vector3(1.5f, 0f, -3.5f),
            new Vector3(2.5f, 0f, 2.5f),
            new Vector3(-2f, 0f, 3.5f)
        };

        AncientSymbol[] symbols = new AncientSymbol[4];
        for (int i = 0; i < 4; i++)
        {
            GameObject sObj = new GameObject($"AncientSymbol_{symNames[i]}");
            sObj.transform.SetParent(symPuzzleObj.transform);
            sObj.transform.position = symPuzzleObj.transform.position + symOffsets[i];

            // Stone Pedestal
            CreatePrimitive(PrimitiveType.Cylinder, "Pedestal", sObj.transform, new Vector3(0f, 0.6f, 0f), new Vector3(0.7f, 0.6f, 0.7f), wallMat);

            // Carved Rune Glyph
            GameObject glyph = CreatePrimitive(PrimitiveType.Cube, "RuneGlyph", sObj.transform,
                new Vector3(0f, 1.4f, 0f), new Vector3(0.55f, 0.55f, 0.15f), mRuneGlow != null ? mRuneGlow : mGold);

            BoxCollider sc = sObj.AddComponent<BoxCollider>();
            sc.isTrigger = true;
            sc.size = new Vector3(1.4f, 2f, 1.4f);
            sc.center = new Vector3(0f, 1f, 0f);

            AncientSymbol asym = sObj.AddComponent<AncientSymbol>();
            asym.symbolIndex = i;
            asym.symbolName = symNames[i];
            asym.puzzleController = sp;
            asym.symbolRenderer = glyph.GetComponent<MeshRenderer>();
            Light sl = sObj.AddComponent<Light>();
            sl.type = LightType.Point;
            sl.range = 3.5f;
            asym.runeLight = sl;

            symbols[i] = asym;
        }
        sp.symbols = symbols;

        // Puzzle 3: Spawn Crystal #3 (Amethyst) on elevated pedestal in Puzzle Grove
        GameObject c3Obj = CreatePrimitive(PrimitiveType.Sphere, "StarrootCrystal_Amethyst", puzzleParent.transform,
            new Vector3(14f, 1.8f, 50f), new Vector3(0.55f, 0.75f, 0.55f), mCrystalAmethyst);
        // Pedestal under crystal
        CreatePrimitive(PrimitiveType.Cylinder, "CrystalPedestal", puzzleParent.transform,
            new Vector3(14f, 0.6f, 50f), new Vector3(0.8f, 0.6f, 0.8f), wallMat);

        crystal3 = c3Obj.AddComponent<StarrootCrystal>();
        crystal3.crystalIndex = 2;
        crystal3.crystalName = "Amethyst Starroot Crystal";
        crystal3.crystalColor = new Color(0.8f, 0.2f, 1f);
        crystal3.crystalRenderer = c3Obj.GetComponent<MeshRenderer>();
        Light l3 = c3Obj.AddComponent<Light>();
        l3.type = LightType.Point;
        l3.range = 5f;
        l3.color = crystal3.crystalColor;
        crystal3.crystalLight = l3;
        SphereCollider sc3 = c3Obj.GetComponent<SphereCollider>();
        if (sc3 != null) sc3.isTrigger = true;

        Debug.Log("Puzzles 1, 2, and Crystal #3 constructed.");
    }

    private static AncientGate BuildAncientGate(Transform parent, Vector3 pos, float rotY, string name, Material mat)
    {
        GameObject gateObj = new GameObject(name);
        gateObj.transform.SetParent(parent);
        gateObj.transform.position = pos;
        gateObj.transform.rotation = Quaternion.Euler(0f, rotY, 0f);

        // Frame Pillars
        CreatePrimitive(PrimitiveType.Cube, "Pillar_Left", gateObj.transform,
            new Vector3(-2.8f, 2.5f, 0f), new Vector3(0.9f, 5f, 0.9f), mat);
        CreatePrimitive(PrimitiveType.Cube, "Pillar_Right", gateObj.transform,
            new Vector3(2.8f, 2.5f, 0f), new Vector3(0.9f, 5f, 0.9f), mat);
        CreatePrimitive(PrimitiveType.Cube, "Lintel", gateObj.transform,
            new Vector3(0f, 5.2f, 0f), new Vector3(6.5f, 0.8f, 1.0f), mat);

        // Sliding Doors
        GameObject doorL = CreatePrimitive(PrimitiveType.Cube, "Door_Left", gateObj.transform,
            new Vector3(-1.25f, 2.3f, 0f), new Vector3(2.4f, 4.5f, 0.35f), mat);
        GameObject doorR = CreatePrimitive(PrimitiveType.Cube, "Door_Right", gateObj.transform,
            new Vector3(1.25f, 2.3f, 0f), new Vector3(2.4f, 4.5f, 0.35f), mat);

        BoxCollider blocker = gateObj.AddComponent<BoxCollider>();
        blocker.size = new Vector3(5.5f, 4.5f, 1.0f);
        blocker.center = new Vector3(0f, 2.25f, 0f);

        AncientGate gate = gateObj.AddComponent<AncientGate>();
        gate.leftDoor = doorL.transform;
        gate.rightDoor = doorR.transform;
        gate.blockingCollider = blocker;
        gate.leftDoorOpenOffset = new Vector3(-2.5f, 0f, 0f);
        gate.rightDoorOpenOffset = new Vector3(2.5f, 0f, 0f);

        return gate;
    }

    private static void BuildStarrootTemple(out AncientGate templeGate, out StarrootRelic relic)
    {
        GameObject templeParent = GameObject.Find("FinalStarrootTemple");
        if (templeParent != null) Object.DestroyImmediate(templeParent);
        templeParent = new GameObject("FinalStarrootTemple");

        Material wallMat = mMossStone != null ? mMossStone : mStone;

        // Final Colossal Gate at (25, 0, 30)
        templeGate = BuildAncientGate(templeParent.transform, new Vector3(25f, 0f, 30f), 90f, "Gate_FinalStarrootTemple", wallMat);

        // Temple Sanctum Dais
        CreatePrimitive(PrimitiveType.Cylinder, "SanctumDais", templeParent.transform,
            new Vector3(32f, 0.3f, 30f), new Vector3(12f, 0.3f, 12f), wallMat);

        // Sanctum Pillars with mystical braziers
        Vector3[] pillarPos = new Vector3[] {
            new Vector3(28f, 0f, 26f),
            new Vector3(36f, 0f, 26f),
            new Vector3(28f, 0f, 34f),
            new Vector3(36f, 0f, 34f)
        };

        foreach (var pp in pillarPos)
        {
            CreatePrimitive(PrimitiveType.Cylinder, "SanctumPillar", templeParent.transform,
                pp + new Vector3(0f, 2.5f, 0f), new Vector3(0.9f, 2.5f, 0.9f), wallMat);

            // Brazier bowl
            CreatePrimitive(PrimitiveType.Cylinder, "BrazierBowl", templeParent.transform,
                pp + new Vector3(0f, 5.1f, 0f), new Vector3(1.2f, 0.2f, 1.2f), mGold);

            // Mystic Fire Light
            GameObject fireLightObj = new GameObject("MysticFireLight");
            fireLightObj.transform.SetParent(templeParent.transform);
            fireLightObj.transform.position = pp + new Vector3(0f, 5.4f, 0f);
            Light fl = fireLightObj.AddComponent<Light>();
            fl.type = LightType.Point;
            fl.range = 8f;
            fl.color = new Color(0.2f, 1f, 0.6f);
            fl.intensity = 2.5f;
        }

        // Central Altar of the Starroot
        GameObject altar = CreatePrimitive(PrimitiveType.Cube, "StarrootAltar", templeParent.transform,
            new Vector3(32f, 0.75f, 30f), new Vector3(2.4f, 0.9f, 2.4f), wallMat);

        // The Legendary Starroot Relic hovering above altar!
        GameObject relicObj = CreatePrimitive(PrimitiveType.Sphere, "TheStarrootRelic", templeParent.transform,
            new Vector3(32f, 1.8f, 30f), new Vector3(0.7f, 0.7f, 0.7f), mRelicGold != null ? mRelicGold : mGold);

        // Orbiting crystalline nodes
        for (int i = 0; i < 3; i++)
        {
            float ang = i * (360f / 3f) * Mathf.Deg2Rad;
            Vector3 nodeOff = new Vector3(Mathf.Cos(ang) * 0.55f, Mathf.Sin(ang * 2f) * 0.15f, Mathf.Sin(ang) * 0.55f);
            CreatePrimitive(PrimitiveType.Sphere, $"RelicNode_{i}", relicObj.transform,
                nodeOff, new Vector3(0.35f, 0.35f, 0.35f), mCrystalEmerald);
        }

        SphereCollider rCol = relicObj.AddComponent<SphereCollider>();
        rCol.isTrigger = true;
        rCol.radius = 1.8f;

        relic = relicObj.AddComponent<StarrootRelic>();
        Light rLight = relicObj.AddComponent<Light>();
        rLight.type = LightType.Point;
        rLight.range = 10f;
        rLight.color = new Color(1f, 0.85f, 0.2f);
        rLight.intensity = 4.5f;
        relic.relicAuraLight = rLight;

        // Reposition existing Goal object right at the altar
        GameObject existingGoal = GameObject.Find("Goal");
        if (existingGoal != null)
        {
            existingGoal.transform.position = new Vector3(32f, 1.5f, 30f);
            existingGoal.transform.localScale = new Vector3(3f, 3f, 3f);
            MeshRenderer mr = existingGoal.GetComponent<MeshRenderer>();
            if (mr != null) mr.enabled = false; // Hide ugly cube mesh
        }

        Debug.Log("Final Starroot Temple & Starroot Relic Altar created.");
    }

    private static GameHUD EnhanceUI()
    {
        GameObject canvasObj = GameObject.Find("Canvas");
        if (canvasObj == null)
        {
            Debug.LogWarning("Canvas not found!");
            return null;
        }

        GameHUD hud = canvasObj.GetComponent<GameHUD>();
        if (hud == null) hud = canvasObj.AddComponent<GameHUD>();

        // Find existing HealthBar slider
        Slider healthSlider = canvasObj.GetComponentInChildren<Slider>();

        // Create HUD Container
        Transform hudContainer = canvasObj.transform.Find("StarrootHUD");
        if (hudContainer != null) Object.DestroyImmediate(hudContainer.gameObject);

        GameObject hudRoot = new GameObject("StarrootHUD", typeof(RectTransform));
        hudRoot.transform.SetParent(canvasObj.transform, false);
        RectTransform rtRoot = hudRoot.GetComponent<RectTransform>();
        rtRoot.anchorMin = Vector2.zero;
        rtRoot.anchorMax = Vector2.one;
        rtRoot.offsetMin = Vector2.zero;
        rtRoot.offsetMax = Vector2.zero;

        // 1. Objective Text (Top Center-Left)
        GameObject objTextObj = new GameObject("ObjectiveText", typeof(RectTransform));
        objTextObj.transform.SetParent(hudRoot.transform, false);
        RectTransform rtObj = objTextObj.GetComponent<RectTransform>();
        rtObj.anchorMin = new Vector2(0f, 1f);
        rtObj.anchorMax = new Vector2(0f, 1f);
        rtObj.pivot = new Vector2(0f, 1f);
        rtObj.anchoredPosition = new Vector2(25f, -65f);
        rtObj.sizeDelta = new Vector2(650f, 40f);
        TextMeshProUGUI tmpObj = objTextObj.AddComponent<TextMeshProUGUI>();
        tmpObj.fontSize = 20;
        tmpObj.color = new Color(1f, 0.95f, 0.75f);
        tmpObj.text = "Objective: Follow the jungle trail to the Ancient Village.";
        hud.objectiveText = tmpObj;

        // 2. Crystal Counter Text (Top Right)
        GameObject cryTextObj = new GameObject("CrystalCounterText", typeof(RectTransform));
        cryTextObj.transform.SetParent(hudRoot.transform, false);
        RectTransform rtCry = cryTextObj.GetComponent<RectTransform>();
        rtCry.anchorMin = new Vector2(1f, 1f);
        rtCry.anchorMax = new Vector2(1f, 1f);
        rtCry.pivot = new Vector2(1f, 1f);
        rtCry.anchoredPosition = new Vector2(-25f, -25f);
        rtCry.sizeDelta = new Vector2(350f, 40f);
        TextMeshProUGUI tmpCry = cryTextObj.AddComponent<TextMeshProUGUI>();
        tmpCry.fontSize = 22;
        tmpCry.alignment = TextAlignmentOptions.Right;
        tmpCry.color = new Color(0.3f, 1f, 0.5f);
        tmpCry.text = "Starroot Crystals: 0 / 3";
        hud.crystalCounterText = tmpCry;

        // 3. Interaction Prompt Text (Bottom Center)
        GameObject promptObj = new GameObject("InteractionPromptText", typeof(RectTransform));
        promptObj.transform.SetParent(hudRoot.transform, false);
        RectTransform rtPrompt = promptObj.GetComponent<RectTransform>();
        rtPrompt.anchorMin = new Vector2(0.5f, 0f);
        rtPrompt.anchorMax = new Vector2(0.5f, 0f);
        rtPrompt.pivot = new Vector2(0.5f, 0f);
        rtPrompt.anchoredPosition = new Vector2(0f, 80f);
        rtPrompt.sizeDelta = new Vector2(500f, 45f);
        TextMeshProUGUI tmpPrompt = promptObj.AddComponent<TextMeshProUGUI>();
        tmpPrompt.fontSize = 24;
        tmpPrompt.alignment = TextAlignmentOptions.Center;
        tmpPrompt.color = Color.white;
        tmpPrompt.text = "[E] Interact";
        hud.interactionPromptText = tmpPrompt;
        promptObj.SetActive(false);

        // 4. Center Notification Banner (Middle)
        GameObject bannerObj = new GameObject("NotificationBannerText", typeof(RectTransform));
        bannerObj.transform.SetParent(hudRoot.transform, false);
        RectTransform rtBanner = bannerObj.GetComponent<RectTransform>();
        rtBanner.anchorMin = new Vector2(0.5f, 0.5f);
        rtBanner.anchorMax = new Vector2(0.5f, 0.5f);
        rtBanner.pivot = new Vector2(0.5f, 0.5f);
        rtBanner.anchoredPosition = new Vector2(0f, 120f);
        rtBanner.sizeDelta = new Vector2(750f, 60f);
        TextMeshProUGUI tmpBanner = bannerObj.AddComponent<TextMeshProUGUI>();
        tmpBanner.fontSize = 28;
        tmpBanner.alignment = TextAlignmentOptions.Center;
        tmpBanner.color = new Color(1f, 0.85f, 0.2f);
        tmpBanner.text = "";
        hud.notificationBannerText = tmpBanner;

        // Connect existing EndPanel and EndText
        GameObject endPanel = GameObject.Find("EndPanel");
        if (endPanel != null)
        {
            hud.gameOverPanel = endPanel;
            TextMeshProUGUI endTmp = endPanel.GetComponentInChildren<TextMeshProUGUI>();
            hud.gameOverText = endTmp;
            hud.victoryPanel = endPanel;
            hud.victoryText = endTmp;
        }

        Debug.Log("UI & HUD successfully configured with TextMeshPro.");
        return hud;
    }

    private static void UpdateGameManager(GameHUD hud, AncientGate finalGate)
    {
        GameObject gmObj = GameObject.Find("GameManager");
        if (gmObj == null) return;

        GameManager gm = gmObj.GetComponent<GameManager>();
        if (gm != null)
        {
            gm.hud = hud;
            gm.finalTempleGate = finalGate;
            gm.timeLeft = 600f; // Generous exploration time
            gm.totalCrystals = 3;
            gm.crystalsCollected = 0;
            gm.playerHealth = Object.FindFirstObjectByType<Health>();
        }

        Debug.Log("GameManager configured with HUD and Final Temple Gate.");
    }

    private static void SetupAtmosphere()
    {
        // Directional Light
        GameObject lightObj = GameObject.Find("Directional Light");
        if (lightObj != null)
        {
            Light sun = lightObj.GetComponent<Light>();
            if (sun != null)
            {
                sun.color = new Color(1.0f, 0.95f, 0.84f);
                sun.intensity = 1.25f;
                sun.shadows = LightShadows.Soft;
            }
            lightObj.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        }

        // Atmosphere Fog
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = 0.007f;
        RenderSettings.fogColor = new Color(0.48f, 0.58f, 0.50f);
        RenderSettings.ambientLight = new Color(0.25f, 0.28f, 0.24f);

        Debug.Log("Jungle lighting and atmospheric fog configured.");
    }

    private static GameObject CreatePrimitive(PrimitiveType type, string name, Transform parent, Vector3 localPos, Vector3 localScale, Material mat)
    {
        GameObject obj = GameObject.CreatePrimitive(type);
        obj.name = name;
        obj.transform.SetParent(parent, false);
        obj.transform.localPosition = localPos;
        obj.transform.localScale = localScale;
        if (mat != null)
        {
            MeshRenderer mr = obj.GetComponent<MeshRenderer>();
            if (mr != null) mr.sharedMaterial = mat;
        }
        return obj;
    }

    private static GameObject CreateStoneArch(Transform parent, Vector3 pos, float rotY, Material mat)
    {
        GameObject arch = new GameObject("AncientStoneArch");
        arch.transform.SetParent(parent);
        arch.transform.position = pos;
        arch.transform.rotation = Quaternion.Euler(0f, rotY, 0f);

        CreatePrimitive(PrimitiveType.Cube, "Pillar_L", arch.transform, new Vector3(-2.2f, 2f, 0f), new Vector3(0.8f, 4f, 0.8f), mat);
        CreatePrimitive(PrimitiveType.Cube, "Pillar_R", arch.transform, new Vector3(2.2f, 2f, 0f), new Vector3(0.8f, 4f, 0.8f), mat);
        CreatePrimitive(PrimitiveType.Cube, "Lintel", arch.transform, new Vector3(0f, 4.4f, 0f), new Vector3(5.2f, 0.8f, 1f), mat);

        return arch;
    }
}
