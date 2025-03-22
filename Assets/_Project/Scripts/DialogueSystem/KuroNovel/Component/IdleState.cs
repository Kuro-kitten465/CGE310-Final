using UnityEngine;

namespace KuroNovel.Component
{
    public class IdleState : IDialogueState
    {
        private DialogueController m_Controller;

        public IdleState(DialogueController controller)
        {
            m_Controller = controller;
        }

        public void OnEnter()
        {
            Debug.Log("Dialogue System is idle, waiting for dialogue to start.");
        }

        public void OnExit()
        {
            Debug.Log("Exiting Idle State.");
        }

        public void OnUpdate()
        {
            
        }
    }
}