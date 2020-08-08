using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MvM
{
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
        public CardStack<CommandCard> commmandCardDeck = new CardStack<CommandCard>();
        public CardStack<CommandCard> commandCardDiscard = new CardStack<CommandCard>();
        public CardStack<DamageCard> damageCardDeck = new CardStack<DamageCard>();
        public CardStack<DamageCard> damageCardDiscard = new CardStack<DamageCard>();
        //public CardStack<BossCard> bossCardDeck = new CardStack<BossCard>();
        //public CardStack<BossCard> bossCardDiscard = new CardStack<BossCard>();

        [HideInInspector]
        public Player localPlayer; // NOTE: For hot seat games, all players are local players. Code should reflect this.
        [HideInInspector]
        public MapSquare interactedSquare;

        public bool commandLineFinished = false;
        public bool minionMovesFinished = false;

        private bool gameStarted = false;
        private bool interactableSquaresMarked = false;
        #endregion

        #region Main Menu and initialization

        private void Start()
        {
            gearTracker.InitRunes();
        }

        private void InitPlayers()
        {
            // Testing code
            Player player = new Player("Player");

            players.Add(player);
            localPlayer = player;
            currentPlayer = player;
        }

        public void InitDecks()
        {
            #region Initialize Command Card Deck
            for (int i = 0; i < 8; i++)
            {
                // Turn cards
                commmandCardDeck.AddTop(new FuelTank());
                commmandCardDeck.AddTop(new Scythe());
                commmandCardDeck.AddTop(new MemoryCore());
                commmandCardDeck.AddTop(new Cyclotron());

                // Move cards
                commmandCardDeck.AddTop(new Blaze());
                commmandCardDeck.AddTop(new Skewer());
                commmandCardDeck.AddTop(new Omnistomp());
                commmandCardDeck.AddTop(new Speed());

                // Attack cards
                commmandCardDeck.AddTop(new Flamespitter());
                commmandCardDeck.AddTop(new Ripsaw());
                commmandCardDeck.AddTop(new HexmaticAimbot());
                commmandCardDeck.AddTop(new ChainLightning());
            }
            commmandCardDeck = ShuffleDeck(commmandCardDeck);
            #endregion

            #region Initialize Damage Card Deck
            for (int i = 0; i < 4; i++)
            {
                //damageCardDeck.AddTop(new Glitch(0, 1));
                //damageCardDeck.AddTop(new Glitch(2, 3));
                //damageCardDeck.AddTop(new Glitch(4, 5));
            }
            for (int i = 0; i < 3; i++)
            {
                //damageCardDeck.AddTop(new MajorGlitch());
                damageCardDeck.AddTop(new StuckControls(Tools.Facing.Left, true));
                damageCardDeck.AddTop(new StuckControls(Tools.Facing.Left, true));
                //damageCardDeck.AddTop(new StuckControls(Tools.Facing.Right, false));
                //damageCardDeck.AddTop(new StuckControls(Tools.Facing.Right, false));
                //damageCardDeck.AddTop(new HaywireControls());
                //damageCardDeck.AddTop(new HaywireRotator());
            }
            for (int i = 0; i < 2; i++)
            {
                damageCardDeck.AddTop(new StuckControls(Tools.Facing.Back, true));
                //damageCardDeck.AddTop(new StuckControls(Tools.Facing.Back, false));
            }

            damageCardDeck = ShuffleDeck(damageCardDeck);
            #endregion

            #region Initialize Boss Card Deck
            // NOTE: Move to Scenario? Move to separate function only called from Scenario?
            //for (int i = 0; i < 2; i++)
            //{
            //bossCardDeck.AddTop(new AxeCannon());
            //bossCardDeck.AddTop(new RunicShrapnel());
            //bossCardDeck.AddTop(new Quadrabeam());
            //bossCardDeck.AddTop(new Recharge());
            //bossCardDeck.AddTop(new Electrostomp());
            //bossCardDeck.AddTop(new Charge());
            //bossCardDeck.AddTop(new GetOverHere());
            //bossCardDeck.AddTop(new MechMagnet());
            //bossCardDeck.AddTop(new Scrambulator());
            //bossCardDeck.AddTop(new FireStorm());
            //bossCardDeck.AddTop(new FlameNova());
            //bossCardDeck.AddTop(new AxeStrike());
            //}
            //bossCardDeck = ShuffleDeck(bossCardDeck);
            #endregion
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

            currentTurnState.StartState();
        }
        #endregion

        #region InGame functionality

        private void Update()
        {
            // If game has started, update the state
            if (gameStarted)
            {
                // First check if game has been won or lost, prioritizing win
                if (scenario.GameWon)
                    currentTurnState = new TurnState_EndGame(true);
                else if (scenario.GameLost)
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
            StartCoroutine(DoExecuteAllCommandLines());
        }

        private IEnumerator DoExecuteAllCommandLines()
        {
            // Handle each player's command line
            foreach (Player player in players)
            {
                currentPlayer = player;
                ExecuteCurrentCommandLine();
                while (!commandLineFinished)
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

        public void ExecuteCurrentCommandLine()
        {
            StartCoroutine(DoExecuteCurrentCommandLine());
        }

        private IEnumerator DoExecuteCurrentCommandLine(bool reverseOrder = false)
        {
            commandLineFinished = false;

            //yield return new WaitForSeconds(Settings.commandExecutionDelay);

            currentPlayer.character.ToggleHighlight(true);
            int end = reverseOrder ? -1 : 6;
            for (int i = reverseOrder ? 5 : 0; (!reverseOrder && i < end) || (reverseOrder && i > end); i = reverseOrder ? i - 1 : i + 1)
            {
                interactableSquaresMarked = false;
                interactedSquare = null;

                cardBeingExecuted = currentPlayer.commandLine.GetTopCard(i);
                if (cardBeingExecuted != null)
                {
                    UIMaster.Instance.cardSlots[i].SetHighlightState(UIHighlight.HighlightState.Available);
                    cardBeingExecuted.InitializeCardExecution(currentPlayer.character); // NOTE: hardcoded for players, requires recoding if used for other units!

                    // If no input is required, skip straight to execution
                    if (cardBeingExecuted.inputRequired)
                    {
                        // While card is not ready to execute, update it and wait for input
                        while (!cardBeingExecuted.readyToExecute)
                        {
                            // Mark interactable squares for the player
                            if (!interactableSquaresMarked)
                            {
                                Dictionary<MapSquare, MapSquare.Interactable> inputSquares = cardBeingExecuted.GetValidInputSquares();
                                // Failsafe
                                if (!inputSquares.Values.Contains(MapSquare.Interactable.Interactable))
                                    cardBeingExecuted.NoViableInputOptions();
                                else
                                    MapInput.Instance.SetInteractables(inputSquares);

                                interactableSquaresMarked = true;
                            }

                            // If we have received input, handle input to the card
                            if (!cardBeingExecuted.inputReceived && interactedSquare)
                            {
                                MapInput.Instance.ClearInteractables();
                                cardBeingExecuted.Input(interactedSquare);
                                interactableSquaresMarked = false;
                                interactedSquare = null;
                            }
                            cardBeingExecuted.UpdateCardState();
                            yield return null;
                        }
                    }
                    cardBeingExecuted.ExecuteCard();
                    yield return new WaitForSeconds(Settings.commandExecutionDelay);
                    while (currentPlayer.character.actionsInProgress)
                        yield return null;
                    UIMaster.Instance.cardSlots[i].SetHighlightState(UIHighlight.HighlightState.Available);
                }
                else
                    yield return null;
                cardBeingExecuted = null;
            }
            commandLineFinished = true;
            currentPlayer.character.ToggleHighlight(false);
            yield return null;
        }

        public void RepairCommandSlot(Player player, int slotIndex)
        {
            // Data handling
            DiscardCard(player.commandLine.cards[slotIndex].PopTop());

            // NOTE: Not sure about this being in here, data and UI should be separated more efficiently
            // UI handling
            UIMaster.Instance.cardSlots[slotIndex].UpdateSlotElements();
        }

        public void SwapSlots(Player player, int slotIndex1, int slotIndex2)
        {
            // Data handling
            CardStack<Card> tempStack = player.commandLine.cards[slotIndex1];
            player.commandLine.cards[slotIndex1] = player.commandLine.cards[slotIndex2];
            player.commandLine.cards[slotIndex2] = tempStack;

            // UI handling
            UIMaster.Instance.cardSlots[slotIndex1].UpdateSlotElements();
            UIMaster.Instance.cardSlots[slotIndex2].UpdateSlotElements();
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

        public void SetLocalPlayerReady(bool ready)
        {
            localPlayer.ready = ready;
        }

        #region Deck & Discard handling
        /// <summary>
        /// Shuffles the deck using the Fisher-Yates algorithm.
        /// </summary>
        public CardStack<T> ShuffleDeck<T>(CardStack<T> deck)
        {
            var cards = deck.ToArray();
            for (int i = cards.Length - 1; i > 0; i--)
            {
                int random = Random.Range(0, i);
                var temp = cards[random];
                cards[random] = cards[i];
                cards[i] = temp;
            }

            CardStack<T> newStack = new CardStack<T>();
            foreach (var card in cards)
            {
                newStack.AddTop(card);
            }

            deck = newStack;

            return deck;
        }

        public (CardStack<T>, CardStack<T>) ResetDeck<T>(CardStack<T> deck, CardStack<T> discard)
        {
            deck = new CardStack<T>(discard);
            discard.Clear();
            ShuffleDeck(deck);

            return (deck, discard);
        }

        public Card DrawCard(Card.Type type)
        {
            Card drawnCard = null;
            switch (type)
            {
                case Card.Type.Command:
                    if (commmandCardDeck.Count < 1)
                        (commmandCardDeck, commandCardDiscard) = ResetDeck(commmandCardDeck, commandCardDiscard);
                    drawnCard = commmandCardDeck.PopTop();
                    break;

                case Card.Type.Damage:
                    if (damageCardDeck.Count < 1)
                        (damageCardDeck, damageCardDiscard) = ResetDeck(damageCardDeck, damageCardDiscard);
                    drawnCard = damageCardDeck.PopTop();
                    break;

                case Card.Type.Boss:
                    break;

                default:
                    break;
            }

            return drawnCard;
        }

        public void DiscardCard(Card card)
        {
            switch (card.type)
            {
                case Card.Type.Command:
                    commandCardDiscard.AddTop(card as CommandCard);
                    return;

                case Card.Type.Damage:
                    damageCardDiscard.AddTop(card as DamageCard);
                    return;

                case Card.Type.Boss:
                    return;

                default:
                    return;
            }
        }
        #endregion

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

        private IEnumerator HandlePlayerDamage(Player player)
        {
            // NOTE: Current solution doesn't handle minion order in any way
            int damageAmount = 0;

            // Take damage for all cardinal directions
            List<Tools.Direction> damageDirections = new List<Tools.Direction> { Tools.Direction.North, Tools.Direction.East, Tools.Direction.West, Tools.Direction.South };
            // if (hasSpecificDamage) // If player has the specific damage that causes them to take diagonal damage
            //  damageDirections.AddRange(new Tools.Direction[] { Tools.Direction.NorthEast, Tools.Direction.NorthWest, Tools.Direction.SouthEast, Tools.Direction.SouthWest });
            foreach (Tools.Direction dir in damageDirections)
                if (player.character.mapSquare.HasNeighbour(dir) && player.character.mapSquare.neighbours[dir].unit is Minion)
                {
                    damageAmount++;
                    Debug.Log("Player is taking damage from Minion on square " + player.character.mapSquare.neighbours[dir]);
                }
            
            for (int i = 0; i < damageAmount; i++)
            {
                player.character.TakeDamage(Tools.Color.None);
                UIMaster.Instance.UpdateCommandLine();          // NOTE: Change to only update the correct player command line
                yield return new WaitForSeconds(1f);
            }
            player.ready = true; // NOTE: Change to rotate current player

            TurnState_MinionAttack turnState = currentTurnState as TurnState_MinionAttack;
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
                //    //if (localPlayer.hand.Count > 0)
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

        public void CardSlotInteracted(UICardSlot slot)
        {
            if (UIMaster.Instance.state == UIMaster.UIState.Slot)
            {
                if ((currentTurnState as TurnState_Draft).draftStage > 0 &&
                    localPlayer.currentCard != null &&
                    localPlayer.commandLine.CanSlotCard(slot.index, localPlayer.currentCard))
                {
                    UIMaster.Instance.handPanel.RemoveCard(localPlayer.currentCard);

                    SlotCard(slot, localPlayer.currentCard);
                    localPlayer.currentCard = null;

                    UIMaster.Instance.draftPanel.CardSlotted();
                }
            }
            else if (UIMaster.Instance.state == UIMaster.UIState.ScrapRepair)
            {
                if (localPlayer.commandLine.cards[slot.index].Count > 0 &&
                    localPlayer.commandLine.cards[slot.index].PeekTop() is DamageCard)
                {
                    RepairCommandSlot(localPlayer, slot.index);
                    UIMaster.Instance.handPanel.RemoveCard(localPlayer.currentCard);
                    DiscardCard(localPlayer.currentCard);
                    UIMaster.Instance.ToggleRepairScrap(localPlayer, false);
                    UIMaster.Instance.draftPanel.CardSlotted();
                    UpdateUIState();
                }
            }
            else if (UIMaster.Instance.state == UIMaster.UIState.ScrapSwap)
            {
                if (localPlayer.commandLine.cards[slot.index].Count == 0 || !(localPlayer.commandLine.cards[slot.index].PeekTop() is DamageCard))
                {
                    UICardSlot slot1 = UIMaster.Instance.SelectedSwapItem;
                    if (slot1 == null)
                        UIMaster.Instance.SwapScrapInteraction(slot);
                    else if (slot1 != slot)
                    {
                        SwapSlots(localPlayer, slot1.index, slot.index);
                        UIMaster.Instance.handPanel.RemoveCard(localPlayer.currentCard);
                        DiscardCard(localPlayer.currentCard);
                        UIMaster.Instance.SelectedSwapItem = null;
                        UIMaster.Instance.ToggleSwapScrap(localPlayer, false);
                        UIMaster.Instance.draftPanel.CardSlotted();
                        UpdateUIState();
                    }
                }
            }
        }

        public void SlotCard(UICardSlot slot, Card card)
        {
            // Data handling
            localPlayer.commandLine.SlotCard(slot.index, card);

            // UI handling
            slot.SlotCard(localPlayer.currentCard);
        }

        public void Scrap()
        {
            if (localPlayer.currentCard == null)
                return;

            Tools.Color scrapColor = (localPlayer.currentCard as CommandCard).cardColor;
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
                    UIMaster.Instance.ToggleRepairScrap(localPlayer, true);
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
                    if (currentPlayer.commandLine.cards[i].Count > 0 && !(currentPlayer.commandLine.cards[i].PeekTop() is DamageCard))
                        unDamagedSlots++;
                }
                if (unDamagedSlots > 0)
                {
                    UIMaster.Instance.state = UIMaster.UIState.ScrapSwap;
                    UIMaster.Instance.ToggleSwapScrap(localPlayer, true);
                }
                else
                {
                    NoScrapEffect();
                }

            }
        }

        private void NoScrapEffect()
        {
            UIMaster.Instance.handPanel.RemoveCard(localPlayer.currentCard);
            DiscardCard(localPlayer.currentCard);
            UIMaster.Instance.ToggleRepairScrap(localPlayer, false);
            UIMaster.Instance.draftPanel.CardSlotted();
            UpdateUIState();
        }

        #endregion
    }
}
