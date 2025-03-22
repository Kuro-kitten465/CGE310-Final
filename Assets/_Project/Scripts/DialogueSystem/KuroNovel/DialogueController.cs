using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Debug;
using KuroNovel.Component;

namespace KuroNovel
{
    public class DialogueController : MonoBehaviour
    {
        public static DialogueController Instance { get; private set; }
        private Dictionary<DialogueStateType, IDialogueState> m_States;
        private IDialogueState m_CurrentState;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            InitializeStates();
        }

        private void InitializeStates()
        {
            m_States = new Dictionary<DialogueStateType, IDialogueState>
            {
                { DialogueStateType.Idle, new IdleState(this) }
                /*{ DialogueStateType.Typing, new TypingState(this) },
                { DialogueStateType.WaitingForInput, new WaitingForInputState(this) },
                { DialogueStateType.Choice, new ChoiceState(this) },
                { DialogueStateType.End, new EndState(this) }*/
            };
            
            ChangeState(DialogueStateType.Idle);
        }

        private void Update()
        {
            
        }

        public void ChangeState(DialogueStateType newState)
        {
            m_CurrentState?.OnExit();
            m_CurrentState = m_States[newState];
            m_CurrentState.OnEnter();
        }
    }
}
