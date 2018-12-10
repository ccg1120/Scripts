public void EventClear()
        {
            Debug.Log("EventClear");
            if (mCallbackEvent == null) return;
            Delegate[] delegates = mCallbackEvent.GetInvocationList();
            foreach (Delegate d in delegates)
            {
                mCallbackEvent -= (JavaCallback)d;
            }
        }
