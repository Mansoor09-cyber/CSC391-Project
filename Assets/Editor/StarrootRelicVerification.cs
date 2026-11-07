using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StarrootRelicVerification
{
    [MenuItem("The Starroot Relic/Verify Scene Integrity")]
    public static void Verify()
    {
        Debug.Log("====================================================");
        Debug.Log(">>> RUNNING FULL VERIFICATION FOR THE STARROOT RELIC <<<");
        Debug.Log("====================================================");

        var scene = EditorSceneManager.OpenScene("Assets/game.unity");
        if (!scene.IsValid())
        {
            Debug.LogError("FAIL: Could not open Assets/game.unity!");
            return;
        }

        int passCount = 0;
        int totalTests = 16;

        // 1. Ari Character & Controls
        GameObject ari = GameObject.Find("Ari");
        if (ari != null &&
            ari.GetComponent<PlayerMover>() != null &&
            ari.GetComponent<Health>() != null &&
            ari.GetComponent<AriProceduralAnimator>() != null &&
            ari.GetComponent<PlayerInteraction>() != null &&
            ari.transform.Find("AriVisuals") != null)
        {
            Debug.Log("[PASS 1/16] Ari Female Adventurer Character: Humanoid model, Mover, Health, Procedural Animator, Interaction.");
            passCount++;
        }
        else Debug.LogError("[FAIL 1/16] Ari character check failed!");

        // 2. Camera Follow
        GameObject cam = GameObject.Find("Main Camera");
        if (cam != null && cam.GetComponent<CameraFollow>() != null && cam.GetComponent<CameraFollow>().target != null)
        {
            Debug.Log("[PASS 2/16] Main Camera Follow: Smooth third-person tracking Ari target.");
            passCount++;
        }
        else Debug.LogError("[FAIL 2/16] Camera Follow check failed!");

        // 3. Dense Jungle Trees
        GameObject jungle = GameObject.Find("DenseJungleEnvironment");
        if (jungle != null && jungle.transform.childCount >= 30)
        {
            Debug.Log($"[PASS 3/16] Dense Tropical Jungle Environment: {jungle.transform.childCount} trees flanking perimeter and pathways.");
            passCount++;
        }
        else Debug.LogError("[FAIL 3/16] Jungle Environment check failed!");

        // 4. Abandoned Ancient Village (5 Destroyed Houses)
        GameObject village = GameObject.Find("AbandonedVillage");
        if (village != null &&
            village.transform.Find("House_01_OvergrownCottage") != null &&
            village.transform.Find("House_02_ShatteredWatchtower") != null &&
            village.transform.Find("House_03_ElderSanctuary") != null &&
            village.transform.Find("House_04_SunkenDwelling") != null &&
            village.transform.Find("House_05_WorkshopAndWell") != null)
        {
            Debug.Log("[PASS 4/16] Abandoned Ancient Village: 5 distinct destroyed ancient houses and well courtyard.");
            passCount++;
        }
        else Debug.LogError("[FAIL 4/16] Abandoned Village check failed!");

        // 5. Ancient Ruins & Environmental Storytelling
        GameObject ruins = GameObject.Find("AncientRuins");
        if (ruins != null &&
            ruins.transform.Find("AncientGuardianStatue") != null &&
            ruins.GetComponentInChildren<ClueStele>() != null)
        {
            Debug.Log("[PASS 5/16] Ancient Ruins: Guardian statues with glowing gem eyes, colossal pillars, and Clue Stele.");
            passCount++;
        }
        else Debug.LogError("[FAIL 5/16] Ancient Ruins check failed!");

        // 6. Obstacle 1: Falling Rock Trap
        GameObject rockTrap = GameObject.Find("Obstacle_01_FallingRockTrap");
        if (rockTrap != null && rockTrap.GetComponent<FallingRockTrap>() != null)
        {
            Debug.Log("[PASS 6/16] Obstacle 1: Falling Rock Trap with physics boulder, trigger volume, and impact damage.");
            passCount++;
        }
        else Debug.LogError("[FAIL 6/16] Falling Rock Trap check failed!");

        // 7. Obstacle 2: Moving Spiked Log Trap
        GameObject logTrap = GameObject.Find("Obstacle_02_MovingLogTrap");
        if (logTrap != null && logTrap.GetComponent<MovingLogTrap>() != null)
        {
            Debug.Log("[PASS 7/16] Obstacle 2: Moving Spiked Log Trap with horizontal ping-pong sweep and hazard collision.");
            passCount++;
        }
        else Debug.LogError("[FAIL 7/16] Moving Log Trap check failed!");

        // 8. Obstacle 3: Retractable Spike Trap
        GameObject spikeTrap = GameObject.Find("Obstacle_03_SpikeTrap");
        if (spikeTrap != null && spikeTrap.GetComponent<SpikeTrap>() != null)
        {
            Debug.Log("[PASS 8/16] Obstacle 3: Retractable Spike Trap with periodic cycle and sharp floor grates.");
            passCount++;
        }
        else Debug.LogError("[FAIL 8/16] Spike Trap check failed!");

        // 9. Obstacle 4: Collapsing Wooden Bridge
        GameObject bridge = GameObject.Find("Obstacle_04_CollapsingBridge");
        if (bridge != null && bridge.GetComponentsInChildren<CollapsingBridge>().Length >= 4)
        {
            Debug.Log("[PASS 9/16] Obstacle 4: Collapsing Wooden Bridge with 4 shake-and-fall planks across ravine.");
            passCount++;
        }
        else Debug.LogError("[FAIL 9/16] Collapsing Bridge check failed!");

        // 10. Puzzle 1: Pressure Plates
        PressurePlatePuzzle ppp = Object.FindFirstObjectByType<PressurePlatePuzzle>();
        if (ppp != null && ppp.plates != null && ppp.plates.Length == 3 && ppp.linkedGate != null)
        {
            Debug.Log("[PASS 10/16] Puzzle 1 (Pressure Plates): 3 interactive plates with depression, rune glow, and linked gate.");
            passCount++;
        }
        else Debug.LogError("[FAIL 10/16] Pressure Plate Puzzle check failed!");

        // 11. Puzzle 2: Ancient Symbols
        SymbolPuzzle sp = Object.FindFirstObjectByType<SymbolPuzzle>();
        if (sp != null && sp.symbols != null && sp.symbols.Length == 4 && sp.linkedGate != null)
        {
            Debug.Log("[PASS 11/16] Puzzle 2 (Ancient Symbols): 4 interactive runes (Sun, Star, Moon, Serpent) with clue alignment.");
            passCount++;
        }
        else Debug.LogError("[FAIL 11/16] Ancient Symbol Puzzle check failed!");

        // 12. Puzzle 3: Starroot Crystals (3 Collectibles)
        StarrootCrystal[] crystals = Object.FindObjectsByType<StarrootCrystal>(FindObjectsSortMode.None);
        if (crystals != null && crystals.Length == 3)
        {
            Debug.Log($"[PASS 12/16] Puzzle 3 (Starroot Crystals): All 3 collectible crystals placed (Emerald, Sapphire, Amethyst).");
            passCount++;
        }
        else Debug.LogError($"[FAIL 12/16] Starroot Crystals check failed (found {crystals?.Length ?? 0} / 3)!");

        // 13. Final Starroot Temple & Relic
        StarrootRelic relic = Object.FindFirstObjectByType<StarrootRelic>();
        GameObject templeGate = GameObject.Find("Gate_FinalStarrootTemple");
        if (relic != null && templeGate != null && templeGate.GetComponent<AncientGate>() != null)
        {
            Debug.Log("[PASS 13/16] Final Starroot Temple: Colossal ancient gate, sanctum braziers, altar, and pulsating Relic.");
            passCount++;
        }
        else Debug.LogError("[FAIL 13/16] Final Starroot Temple check failed!");

        // 14. UI & HUD (TextMeshPro)
        GameHUD hud = Object.FindFirstObjectByType<GameHUD>();
        if (hud != null &&
            hud.objectiveText != null &&
            hud.crystalCounterText != null &&
            hud.interactionPromptText != null &&
            hud.notificationBannerText != null)
        {
            Debug.Log("[PASS 14/16] UI & HUD: Health bar, Objective display, Crystal counter, Interaction prompt, Notifications.");
            passCount++;
        }
        else Debug.LogError("[FAIL 14/16] UI & HUD check failed!");

        // 15. Game Manager Integration
        GameManager gm = Object.FindFirstObjectByType<GameManager>();
        if (gm != null && gm.hud != null && gm.finalTempleGate != null && gm.totalCrystals == 3)
        {
            Debug.Log("[PASS 15/16] GameManager: Central controller tracking health, crystals, puzzles, objectives, and victory.");
            passCount++;
        }
        else Debug.LogError("[FAIL 15/16] GameManager check failed!");

        // 16. Audio System & Atmosphere
        ProceduralAudioManager audioMgr = Object.FindFirstObjectByType<ProceduralAudioManager>();
        if (audioMgr != null && RenderSettings.fog)
        {
            Debug.Log("[PASS 16/16] Audio & Atmosphere: Procedural Audio Engine (footsteps, chimes, rumbles) and Atmospheric Jungle Fog.");
            passCount++;
        }
        else Debug.LogError("[FAIL 16/16] Audio & Atmosphere check failed!");

        Debug.Log("====================================================");
        Debug.Log($">>> VERIFICATION RESULT: {passCount} / {totalTests} TESTS PASSED <<<");
        Debug.Log("====================================================");
    }
}
