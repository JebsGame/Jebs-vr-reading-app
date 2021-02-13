using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Events;
using JebsReadingGame.Skeletons;
using JebsReadingGame.Events;

namespace JebsReadingGame.Skeletons
{
    // Global classes
    public class GlobalModel : MonoBehaviour { }
    public class GlobalView  : MonoBehaviour{ }
    public class GlobalController : MonoBehaviour { }

    // API response format
    public struct IpInfo
    {
        public string ip;
    }

    // Persistent model: Persistent between scenes
    public class Persistent
    {
        string persistentValueKey = "key";

        public int persistentValue
        {
            get { return PlayerPrefs.GetInt(persistentValueKey); }
            set { PlayerPrefs.SetInt(persistentValueKey, value); }
        }

        public StringEvent onIpReceived = new StringEvent();

        public void GetMyIpAsync()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.ipify.org/?format=json"));
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            // This can take long

            string jsonResponse = reader.ReadToEnd();
            IpInfo myIp = JsonUtility.FromJson<IpInfo>(jsonResponse);

            onIpReceived.Invoke(myIp.ip);
        }
    }

    // Configuration model: Can be shared between models. Cannot be modified in build
    [CreateAssetMenu(menuName = "JesbReadingGame/Skeleton Asset", order = 1)]
    public class Asset : ScriptableObject
    {
        public int configurationValue = 0;
    }

    // Scene-specific model
    public abstract class ParentModel : GlobalModel
    {
        [HideInInspector]
        public ParentView view; // Optional

        public Asset asset;

        public Persistent persistent = new Persistent();

        public float readonlyValue = 0.0f;
        public float modifiableValue = 0.0f;

        public ChildModel[] children;

        // If generic system
        // public List<GlobalView> registry = new List<GlobalView>();

        // If specific system (module)
        // public GenericModel generic;
    }

    public abstract class ChildModel : GlobalModel
    {
        [HideInInspector]
        public ParentModel parent;

        public int childValue = 0;
    }

    // View
    public abstract class ParentView : GlobalView
    {
        // Singleton
        static ParentView _singleton;
        public static ParentView singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = GameObject.FindObjectOfType<ParentView>();

                return _singleton;
            }
        }

        // ViewModel
        public sealed class ViewModel
        {
            ParentModel model;

            // Properties
            public int persistentValue { get { return model.persistent.persistentValue; } }
            public int configurationValue { get { return model.asset.configurationValue; } }
            public float readonlyValue { get { return model.readonlyValue; } }
            public float modifiableValue
            {
                get { return model.modifiableValue; }
                set
                {
                    model.modifiableValue = value;
                    model.view.onValueChanged.Invoke(value);
                }
            }
            public ChildViewModel firstChild { get { return model.children.Length > 0 ? new ChildViewModel(model.children[0]) : null; } }
            public ChildViewModel[] children { get { return GetChildViewModelsArray(); } }
            // public GlobalView[] registry { get { return model.registry.ToArray(); } }

            // Constructor
            public ViewModel(ParentModel model)
            {
                this.model = model;
            }

            // ChildViewModel instantiations
            ChildViewModel[] GetChildViewModelsArray()
            {
                List<ChildViewModel> childrenViewModelsList = new List<ChildViewModel>();

                for (int i = 0; i < model.children.Length; i++)
                {
                    if (model.children[i] != null)
                        childrenViewModelsList.Add(new ChildViewModel(model.children[i]));
                }

                return childrenViewModelsList.ToArray();
            }
        }
        public ViewModel viewModel;

        public sealed class ChildViewModel
        {
            ChildModel model;

            // Properties
            public int childValue { get { return model.childValue; } }

            // Constructor
            public ChildViewModel(ChildModel model)
            {
                this.model = model;
            }
        }

        // Events
        public FloatEvent onValueChanged = new FloatEvent();
        public UnityEvent onOtherEvent = new UnityEvent();
    }

    // Controller
    public class ParentController : GlobalController
    {
        public ParentModel model;
        public ParentView view;

        UnityAction<string> onIpReceived;

        private void Awake()
        {
            // Components connection
            view.viewModel = new ParentView.ViewModel(model);

            // Models hierarchy
            for (int i = 0; i < model.children.Length; i++)
            {
                model.children[i].parent = model;
            }

            model.view = view; // Optional

            // If specific system (module)
            // generic.registry.Add(view);
        }

        private void Start()
        {
            // Listen to events invoked by other systems

            /*
			
			model.viewFromOtherSystem.onOtherEvent.AddListener(DoSth)
			
			or
			
			OtherSystemView.singleton.onOtherEvent.AddListener(DoSth)
			
			*/

            // Async persistent data access
            model.persistent.GetMyIpAsync();

            onIpReceived = new UnityAction<string>((string myIp) =>
            {
                Debug.Log("My IP is " + myIp);
                model.persistent.onIpReceived.RemoveListener(onIpReceived);
            });

            model.persistent.onIpReceived.AddListener(onIpReceived);
        }

        void DoSth()
        {
            Debug.Log("Done!");
        }
    }
}