using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MvM
{
    public class PlayerMovement : MonoBehaviour
    {
        public Unit player;

        private bool inputEnabled = true;

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
                DamageCard dmg = GameMaster.Instance.DrawCard(Card.Type.Damage) as DamageCard;
                dmg.ExecuteCard();
                GameMaster.Instance.DiscardCard(dmg);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Card drawnDamage = GameMaster.Instance.DrawCard(Card.Type.Damage);
                GameMaster.Instance.currentPlayer.commandLine.SlotCard(2, drawnDamage);
                UIMaster.Instance.cardSlots[2].SlotCard(drawnDamage);
            }
        }

        private void Move(Tools.Direction dir)
        {
            player.Move(dir);
            player.ExecuteActions();
            StartCoroutine(DisableInput());
        }

        private void MoveMultiple(Tools.Direction dir)
        {
            player.Move(dir);
            player.Move(dir);
            player.Move(dir);
            player.ExecuteActions();
            StartCoroutine(DisableInput());
        }

        private void Rotate(Tools.Direction dir)
        {
            player.Turn(dir);
            player.ExecuteActions();
            StartCoroutine(DisableInput());
        }

        private void MoveAndRotate(Tools.Direction dir1, Tools.Direction dir2, Tools.Direction dir3)
        {
            player.Move(dir1);
            player.Turn(dir2);
            player.Move(dir3);
            player.ExecuteActions();
            StartCoroutine(DisableInput());
        }

        private IEnumerator DisableInput()
        {
            inputEnabled = false;
            while (player.actionsInProgress)
                yield return null;
            inputEnabled = true;
        }
    }
}
