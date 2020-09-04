using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class PlayerMovement : MonoBehaviour
    {
        private bool inputEnabled = true;

        private Unit Player
        {
            get
            {
                return GameMaster.Instance.currentPlayer.character;
            }
        }

        void Update()
        {
            if (!inputEnabled) return;
            // Move single
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Move(Tools.Direction.East);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Move(Tools.Direction.West);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Move(Tools.Direction.North);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Move(Tools.Direction.South);
            }

            // Move multiple
            if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                MoveMultiple(Tools.Direction.East);
            }
            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                MoveMultiple(Tools.Direction.West);
            }
            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                MoveMultiple(Tools.Direction.North);
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                MoveMultiple(Tools.Direction.South);
            }

            // Turn
            if (Input.GetKeyDown(KeyCode.D))
            {
                Rotate(Tools.Direction.East);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                Rotate(Tools.Direction.West);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                Rotate(Tools.Direction.North);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Rotate(Tools.Direction.South);
            }

            // Complex commands
            if (Input.GetKeyDown(KeyCode.Alpha1))
                MoveAndRotate(Tools.Direction.South, Tools.Direction.South, Tools.Direction.East);

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                DamageCard dmg = GameMaster.Instance.damageCardDeck.DrawCard();
                dmg.ExecuteCard();
                GameMaster.Instance.damageCardDeck.DiscardCard(dmg);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Card drawnDamage = GameMaster.Instance.damageCardDeck.DrawCard();
                GameMaster.Instance.currentPlayer.commandLine.SlotCard(2, drawnDamage);
                UIMaster.Instance.cardSlots[2].SlotCard(drawnDamage);
            }

            if (Input.GetKeyDown(KeyCode.KeypadMinus))
                KillAllMinions();
        }

        private void Move(Tools.Direction dir)
        {
            Player.Move(dir);
            Player.ExecuteActions();
            StartCoroutine(DisableInput());
        }

        private void MoveMultiple(Tools.Direction dir)
        {
            Player.Move(dir);
            Player.Move(dir);
            Player.Move(dir);
            Player.ExecuteActions();
            StartCoroutine(DisableInput());
        }

        private void Rotate(Tools.Direction dir)
        {
            Player.Turn(dir);
            Player.ExecuteActions();
            StartCoroutine(DisableInput());
        }

        private void MoveAndRotate(Tools.Direction dir1, Tools.Direction dir2, Tools.Direction dir3)
        {
            Player.Move(dir1);
            Player.Turn(dir2);
            Player.Move(dir3);
            Player.ExecuteActions();
            StartCoroutine(DisableInput());
        }

        private IEnumerator DisableInput()
        {
            inputEnabled = false;
            while (Player.actionsInProgress)
                yield return null;
            inputEnabled = true;
        }

        private void KillAllMinions()
        {
            for (int i = GameMaster.Instance.gearTracker.minionsList.Count - 1; i <= 0; i--)
            {
                GameMaster.Instance.gearTracker.minionsList[i].mapSquare.unit = null;
                Destroy(GameMaster.Instance.gearTracker.minionsList[i].gameObject);
            }
        }
    }
}
