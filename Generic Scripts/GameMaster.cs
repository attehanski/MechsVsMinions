﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MvM
{
    // TODO: Split into smaller pieces
    public class GameMaster : Singleton<GameMaster>
    {
        [System.Serializable]
        public class GearTracker
        {
            public int minionsKilled;
            [Range(0, 15)]
            public int doomTracker;
            public Dictionary<Tools.Color, bool> runes = new Dictionary<Tools.Color, bool>();
            public List<Unit> minionsList = new List<Unit>();

            //[Header("Values specific to some scenarios")] // NOTE: This could possibly be done in a better way

            [System.Serializable]
            public class Rune
            {
                public Tools.Color color;
                public bool open = false;
            }

            public bool RuneOpen(Tools.Color runeColor)
            {
                if (!runes.ContainsKey(runeColor))
                {
                    Debug.LogError("No correct rune color when getting RuneOpen!");
                    return false;
                }
                return runes[runeColor];
            }

            public void SetRuneOpen(Tools.Color runeColor, bool open)
            {
                if (!runes.ContainsKey(runeColor))
                {
                    Debug.LogError("No correct rune color when setting RuneOpen!");
                    return;
                }
                runes[runeColor] = open;
            }

            public void InitRunes()
            {
                runes.Add(Tools.Color.Red, false);
                runes.Add(Tools.Color.Green, false);
                runes.Add(Tools.Color.Blue, false);
                runes.Add(Tools.Color.Yellow, false);
            }

            public void MinionKilled(Unit minion)
            {
                minionsList.Remove(minion);
                minionsKilled++;
            }
        }

        #region Variables
        public int turnNumber = 1;
        public TurnState currentTurnState;
        public GearTracker gearTracker;


        [Header("Runtime variables")]
        public List<Player> players = new List<Player>();
        public Scenario scenario;
        public Player currentPlayer;
        public Card cardBeingExecuted;
        public Deck<CommandCard> commandCardDeck = new Deck<CommandCard>();
        public Deck<DamageCard> damageCardDeck = new Deck<DamageCard>();
        //public Deck<BossCard> bossCardDeck = new CardStack<BossCard>();
        
        [HideInInspector]
        public MapSquare interactedSquare;

        public bool commandLineFinished = false;
        public bool minionMovesFinished = false;

        private bool gameStarted = false;
        private bool interactableSquaresMarked = false;
        private Coroutine commandlineExecutionRoutine;
        #endregion

        #region Main Menu and initialization

        private void Start()
        {
            gearTracker.InitRunes();
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        private void InitPlayers()
        {
            // Testing code
            Player player = new Player("Player");

            players.Add(player);
            currentPlayer = player;
        }

        public void InitDecks()
        {
            #region Initialize Command Card Deck
            for (int i = 0; i < 8; i++)
            {
                // Turn cards
                commandCardDeck.AddCardToDeck(new FuelTank());
                commandCardDeck.AddCardToDeck(new Scythe());
                commandCardDeck.AddCardToDeck(new MemoryCore());
                commandCardDeck.AddCardToDeck(new Cyclotron());

                // Move cards
                commandCardDeck.AddCardToDeck(new Blaze());
                commandCardDeck.AddCardToDeck(new Skewer());
                commandCardDeck.AddCardToDeck(new Omnistomp());
                commandCardDeck.AddCardToDeck(new Speed());

                // Attack cards
                commandCardDeck.AddCardToDeck(new Flamespitter());
                commandCardDeck.AddCardToDeck(new Ripsaw());
                commandCardDeck.AddCardToDeck(new HexmaticAimbot());
                commandCardDeck.AddCardToDeck(new ChainLightning());
            }
            commandCardDeck.ShuffleDeck();
            #endregion

            #region Initialize Damage Card Deck
            for (int i = 0; i < 4; i++)
            {
                damageCardDeck.AddCardToDeck(new Glitch(0, 1));
                damageCardDeck.AddCardToDeck(new Glitch(2, 3));
                damageCardDeck.AddCardToDeck(new Glitch(4, 5));
            }
            for (int i = 0; i < 3; i++)
            {
                damageCardDeck.AddCardToDeck(new MajorGlitch());
                damageCardDeck.AddCardToDeck(new StuckControls(Tools.Facing.Left, true));
                damageCardDeck.AddCardToDeck(new StuckControls(Tools.Facing.Left, true));
                damageCardDeck.AddCardToDeck(new StuckControls(Tools.Facing.Right, false));
                damageCardDeck.AddCardToDeck(new StuckControls(Tools.Facing.Right, false));
                damageCardDeck.AddCardToDeck(new HaywireControls());
                damageCardDeck.AddCardToDeck(new HaywireRotator());
            }
            for (int i = 0; i < 2; i++)
            {
                damageCardDeck.AddCardToDeck(new StuckControls(Tools.Facing.Back, true));
                damageCardDeck.AddCardToDeck(new StuckControls(Tools.Facing.Back, false));
            }
            // if (unlockedForScenario)
            damageCardDeck.AddCardToDeck(new RocketWhoopsie());
            //dmgCardDeck.AddCardToDeck(new CatastrophicFailure()); // TODO: Requires fixing
            //dmgCardDeck.AddCardToDeck(new BeamMisfire());

            damageCardDeck.ShuffleDeck();
            #endregion

            #region Initialize Boss Card Deck
            // NOTE: Move to Scenario? Move to separate function only called from Scenario? Move to a separate Decks class?
            //for (int i = 0; i < 2; i++)
            //{
            //bossCardDeck.AddCardToDeck(new AxeCannon());
            //bossCardDeck.AddCardToDeck(new RunicShrapnel());
            //bossCardDeck.AddCardToDeck(new Quadrabeam());
            //bossCardDeck.AddCardToDeck(new Recharge());
            //bossCardDeck.AddCardToDeck(new Electrostomp());
            //bossCardDeck.AddCardToDeck(new Charge());
            //bossCardDeck.AddCardToDeck(new GetOverHere());
            //bossCardDeck.AddCardToDeck(new MechMagnet());
            //bossCardDeck.AddCardToDeck(new Scrambulator());
            //bossCardDeck.AddCardToDeck(new FireStorm());
            //bossCardDeck.AddCardToDeck(new FlameNova());
            //bossCardDeck.AddCardToDeck(new AxeStrike());
            //}
            //bossCardDeck.ShuffleDeck();
            #endregion
        }

        public void ResetDecks()
        {
            commandCardDeck.Clear();
            damageCardDeck.Clear();
            //bossCardDeck.Clear();
            //bossCardDiscard.Clear();
        }

        public void SetScenario(Scenario newScenario)
        {
            scenario = newScenario;
        }

        public void StartGame()
        {
            scenario.GenerateMap();
            scenario.InitScenario();
            currentTurnState = new TurnState_PreGame();
            InitDecks();
            InitPlayers();
            UIMaster.Instance.ShowInGamePanel();
            UIMaster.Instance.ShowScenarioRules(scenario);
            gameStarted = true;
            turnNumber = 1;

            currentTurnState.StartState();
        }

        public void EndGame()
        {
            StopAllCoroutines();
            //if (commandlineExecutionRoutine != null)
            //    StopCoroutine(commandlineExecutionRoutine);
            foreach (Unit unit in FindObjectsOfType<Unit>())
                Destroy(unit.gameObject);
            gearTracker.minionsKilled = 0;
            gearTracker.minionsList.Clear();

            scenario.RemoveMap();
            scenario = null;
            currentTurnState = null;
            ResetDecks();
            foreach (Player player in players)
                player.ResetPlayer();
            players.Clear();
            gameStarted = false;
        }
        #endregion

        #region InGame functionality

        private void Update()
        {
            // If game has started, update the state
            if (gameStarted)
            {
                // First check if game has been won or lost, prioritizing win
                if (scenario.IsGameWon)
                    currentTurnState = new TurnState_EndGame(true);
                else if (scenario.IsGameLost)
                    currentTurnState = new TurnState_EndGame(false);

                // If all players are done, inform the state
                if (GetAllPlayersReady())
                {
                    currentTurnState.AllPlayersReady();
                    return;
                }

                currentTurnState.UpdateState();
            }
        }

        public void NextPlayer()
        {
            if (players.IndexOf(currentPlayer) == players.Count - 1)
                currentPlayer = players[0];
            else
                currentPlayer = players[players.IndexOf(currentPlayer) + 1];
        }

        #region Command Line functionality
        public void ExecuteAllCommandLines()
        {
            commandlineExecutionRoutine = StartCoroutine(DoExecuteAllCommandLines());
        }

        private IEnumerator DoExecuteAllCommandLines()
        {
            // Handle each player's command line
            foreach (Player player in players)
            {
                currentPlayer = player;
                ExecuteCurrentCommandLine(player);
                while (!player.commandLine.commandLineFinished)
                    yield return null;

                UIMaster.Instance.UpdateMultiButtonState(UIMultiButton.MultiButtonState.Ready);

                while (!currentPlayer.ready)
                    yield return null;

                currentPlayer.ready = false;
            }

            UIMaster.Instance.UpdateMultiButtonState(UIMultiButton.MultiButtonState.Ready);

            while (!GetAllPlayersReady())
                yield return null;

            currentTurnState.AdvanceState();
        }

        public void ExecuteCurrentCommandLine(Player player)
        {
            StartCoroutine(player.commandLine.ExecuteCurrentCommandLine(player));
        }

        public void RepairCommandSlot(Player player, int slotIndex)
        {
            // Data handling
            damageCardDeck.DiscardCard(player.commandLine.cards[slotIndex].PopTop() as DamageCard);

            // NOTE: Not sure about this being in here, data and UI should be separated more efficiently
            // UI handling
            UIMaster.Instance.cardSlots[slotIndex].RepairSlot();
        }

        public void SwapSlots(Player player, int slotIndex1, int slotIndex2)
        {
            // Data handling
            CardStack<Card> tempStack = player.commandLine.cards[slotIndex1];
            player.commandLine.cards[slotIndex1] = player.commandLine.cards[slotIndex2];
            player.commandLine.cards[slotIndex2] = tempStack;

            // UI handling
            UIMaster.Instance.cardSlots[slotIndex1].SwapCards(UIMaster.Instance.cardSlots[slotIndex2]);
        }
        #endregion

        public void MapSquareInteracted(MapSquare square)
        {
            interactedSquare = square;
        }

        public void FinishState()
        {
            SetAllPlayersReady(false);
            currentTurnState.stateFinished = true;
        }

        public bool GetAllPlayersReady()
        {
            bool allPlayersReady = true;
            foreach (Player player in players)
                if (!player.ready)
                    allPlayersReady = false;

            return allPlayersReady;
        }

        public void SetAllPlayersReady(bool readyState)
        {
            foreach (Player player in players)
                player.ready = readyState;
        }

        #region Unit handling
        public Unit SpawnUnit(GameObject spawningUnit, MapSquare square)
        {
            // If square already has a unit, don't spawn
            if (square.unit) return null;

            Unit unit = Instantiate(spawningUnit, square.transform.position, Quaternion.identity).GetComponent<Unit>();
            square.unit = unit;
            unit.mapSquare = square;

            return unit;
        }

        public void MoveMinions(Stack<KeyValuePair<Unit, Tools.Direction>> minionMoves)
        {
            StartCoroutine(DoMoveMinions(minionMoves));
        }

        private IEnumerator DoMoveMinions(Stack<KeyValuePair<Unit, Tools.Direction>> minionMoves)
        {
            minionMovesFinished = false;
            while (minionMoves.Count > 0)
            {
                KeyValuePair<Unit, Tools.Direction> minionMove = minionMoves.Pop();
                minionMove.Key.ToggleHighlight(true);
                minionMove.Key.Move(minionMove.Value);
                minionMove.Key.ExecuteActions();
                while (minionMove.Key.actionsInProgress)
                    yield return null;
                yield return new WaitForSeconds(Settings.minionMoveDelay);
                minionMove.Key.ToggleHighlight(false);
            }
            yield return null;
            minionMovesFinished = true;
        }
        #endregion

        public void DoPlayerDamage(Player player)
        {
            StartCoroutine(HandlePlayerDamage(player));
        }

        // TODO: Change so this works better when taking damage outside of minion attack turn
        private IEnumerator HandlePlayerDamage(Player player)
        {
            // NOTE: Current solution doesn't handle minion order in any way
            int damageAmount = 0;

            // Take damage for all cardinal directions
            List<Tools.Direction> damageDirections = new List<Tools.Direction> { Tools.Direction.North, Tools.Direction.East, Tools.Direction.West, Tools.Direction.South };
            // if (hasSpecificDamage) // TODO: If player has the specific damage that causes them to take diagonal damage, to be added later
            //  damageDirections.AddRange(new Tools.Direction[] { Tools.Direction.NorthEast, Tools.Direction.NorthWest, Tools.Direction.SouthEast, Tools.Direction.SouthWest });
            foreach (Tools.Direction dir in damageDirections)
                if (player.character.mapSquare.HasNeighbour(dir) && player.character.mapSquare.neighbours[dir].unit is Minion)
                {
                    damageAmount++;
                    Debug.Log("Player is taking damage from Minion on square " + player.character.mapSquare.neighbours[dir]);
                }
            
            for (int i = 0; i < damageAmount; i++)
            {
                player.ready = false;
                player.character.TakeDamage(Tools.Color.None);
                yield return UIMaster.Instance.ShowDamageCard(damageCardDeck.discard.PeekTop(), player); // NOTE: Using the top card from discard is a hacky hack
            }
            
            player.ready = true;
            TurnState_MinionAttack turnState = currentTurnState as TurnState_MinionAttack; // TODO: This needs to go as player can take damage outside of minion attack
            turnState.damageInAction = false;

            yield return null;
        }

        public Tools.Direction GetRandomDirection()
        {
            // NOTE: Could change this to roll a color and get direction from compass for scenario specific differences
            Tools.Direction[] dirOptions = new Tools.Direction[] { Tools.Direction.North, Tools.Direction.South, Tools.Direction.East, Tools.Direction.West };
            Tools.Direction randomDir = dirOptions[Random.Range(0, 4)];

            // TODO: Add UI functionality
            return randomDir;
        }

        public int GetDiceRoll()
        {
            int rand = Random.Range(0, 6);

            // TODO: Add UI functionality
            return rand;
        }

        #endregion

        #region Experimental UI interaction

        // NOTE: This should be removed in lieu of a better system.
        public void UpdateUIState()
        {
            if (currentTurnState is TurnState_Draft)
            {
                if ((currentTurnState as TurnState_Draft).draftStage == 0)
                    UIMaster.Instance.state = UIMaster.UIState.Draft;
                else
                    UIMaster.Instance.state = UIMaster.UIState.Slot;

                //if ((UIMaster.Instance.state == UIMaster.UIState.ScrapRepair || UIMaster.Instance.state == UIMaster.UIState.ScrapSwap))
                //{
                //    //if (currentPlayer.hand.Count > 0)
                //    UIMaster.Instance.state = UIMaster.UIState.Slot;
                //    //else
                //    //    UIMaster.Instance.state = UIMaster.UIState.Wait;
                //}
            }
            else if (currentTurnState is TurnState_Players)
                UIMaster.Instance.state = UIMaster.UIState.CmdLine;
            else if (currentTurnState is TurnState_PlayerSpawn)
                UIMaster.Instance.state = UIMaster.UIState.PlayerSpawn;
        }

        // TODO: Cut into smaller parts
        public void CardSlotInteracted(UICardSlot slot)
        {
            if (UIMaster.Instance.state == UIMaster.UIState.Slot)
            {
                if ((currentTurnState as TurnState_Draft).draftStage > 0 &&
                    currentPlayer.currentCard != null &&
                    currentPlayer.commandLine.CanSlotCard(slot.index, currentPlayer.currentCard))
                {
                    currentPlayer.hand.Remove(currentPlayer.currentCard);
                    currentPlayer.commandLine.SlotCard(slot.index, currentPlayer.currentCard);
                    currentPlayer.currentCard = null;

                    UIMaster.Instance.handPanel.SlotCurrentCard(slot);
                    (currentTurnState as TurnState_Draft).CardSlotted();
                }
            }
            else if (UIMaster.Instance.state == UIMaster.UIState.ScrapRepair)
            {
                if (currentPlayer.commandLine.cards[slot.index].Count > 0 &&
                    currentPlayer.commandLine.cards[slot.index].PeekTop() is DamageCard)
                {
                    RepairCommandSlot(currentPlayer, slot.index);
                    UIMaster.Instance.handPanel.RemoveCard(currentPlayer.currentCard);
                    commandCardDeck.DiscardCard(currentPlayer.currentCard as CommandCard);
                    UIMaster.Instance.ToggleRepairScrap(currentPlayer, false);
                    (currentTurnState as TurnState_Draft).CardSlotted();
                    UpdateUIState();
                }
            }
            else if (UIMaster.Instance.state == UIMaster.UIState.ScrapSwap)
            {
                if (currentPlayer.commandLine.cards[slot.index].Count == 0 || !(currentPlayer.commandLine.cards[slot.index].PeekTop() is DamageCard))
                {
                    UICardSlot slot1 = UIMaster.Instance.SelectedSwapItem;
                    if (slot1 == null)
                        UIMaster.Instance.SwapScrapInteraction(slot);
                    else if (slot1 != slot)
                    {
                        SwapSlots(currentPlayer, slot1.index, slot.index);
                        currentPlayer.hand.Remove(currentPlayer.currentCard);
                        UIMaster.Instance.handPanel.RemoveCard(currentPlayer.currentCard);
                        commandCardDeck.DiscardCard(currentPlayer.currentCard as CommandCard);
                        UIMaster.Instance.SelectedSwapItem = null;
                        UIMaster.Instance.ToggleSwapScrap(currentPlayer, false);
                        (currentTurnState as TurnState_Draft).CardSlotted();
                        UpdateUIState();
                    }
                }
            }
        }

        public void SlotCard(UICardSlot slot, Card card)
        {
            // Data handling
            currentPlayer.commandLine.SlotCard(slot.index, card);

            // UI handling
            slot.SlotCard(card);
        }

        // TODO: Cut into smaller pieces
        public void Scrap()
        {
            if (currentPlayer.currentCard == null)
                return;

            Tools.Color scrapColor = (currentPlayer.currentCard as CommandCard).cardColor;
            if (scrapColor == Tools.Color.Red || scrapColor == Tools.Color.Blue)
            {
                int damageOptions = 0;
                for (int i = 0; i < currentPlayer.commandLine.cards.Length; i++)
                {
                    if (currentPlayer.commandLine.cards[i].Count > 0 && currentPlayer.commandLine.cards[i].PeekTop() is DamageCard)
                        damageOptions++;
                }
                if (damageOptions > 0)
                {
                    UIMaster.Instance.state = UIMaster.UIState.ScrapRepair;
                    UIMaster.Instance.ToggleRepairScrap(currentPlayer, true);
                }
                else
                {
                    NoScrapEffect();
                }
            }

            else if (scrapColor == Tools.Color.Yellow || scrapColor == Tools.Color.Green)
            {
                int unDamagedSlots = 0;
                for (int i = 0; i < currentPlayer.commandLine.cards.Length; i++)
                {
                    if (currentPlayer.commandLine.cards[i].Count == 0 || !(currentPlayer.commandLine.cards[i].PeekTop() is DamageCard))
                        unDamagedSlots++;
                }

                if (unDamagedSlots > 1)
                {
                    UIMaster.Instance.state = UIMaster.UIState.ScrapSwap;
                    UIMaster.Instance.ToggleSwapScrap(currentPlayer, true);
                }
                else
                {
                    NoScrapEffect();
                }
            }
        }

        private void NoScrapEffect()
        {
            UIMaster.Instance.handPanel.RemoveCard(currentPlayer.currentCard);
            commandCardDeck.DiscardCard(currentPlayer.currentCard as CommandCard);
            UIMaster.Instance.ToggleRepairScrap(currentPlayer, false);
            (currentTurnState as TurnState_Draft).CardSlotted();
            UpdateUIState();
        }

        #endregion
    }
}
