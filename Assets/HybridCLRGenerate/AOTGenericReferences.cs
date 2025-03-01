using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"System.Core.dll",
		"UniTask.dll",
		"UnityEngine.CoreModule.dll",
		"UnityEngine.UI.dll",
		"aot.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// Aot.core.Singleton<object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<HotFix.core.HotFixEntry.<LoadMetadata>d__2>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<HotFix.core.HotFixEntry.<LoadMetadata>d__2>
	// Cysharp.Threading.Tasks.ITaskPoolNode<object>
	// Cysharp.Threading.Tasks.UniTaskCompletionSourceCore<Cysharp.Threading.Tasks.AsyncUnit>
	// System.Action<UnityEngine.EventSystems.RaycastResult>
	// System.Action<object,object>
	// System.Action<object>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.Comparer<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.HashSet.Enumerator<object>
	// System.Collections.Generic.HashSet<object>
	// System.Collections.Generic.HashSetEqualityComparer<object>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.IComparer<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IList<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.List.Enumerator<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.ObjectComparer<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Comparison<UnityEngine.EventSystems.RaycastResult>
	// System.Comparison<object>
	// System.Func<Cysharp.Threading.Tasks.UniTask>
	// System.Func<int>
	// System.Func<object,object>
	// System.Func<object>
	// System.Predicate<UnityEngine.EventSystems.RaycastResult>
	// System.Predicate<object>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<object>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<object>
	// System.Runtime.CompilerServices.TaskAwaiter<object>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<object>
	// System.Threading.Tasks.Task<object>
	// System.Threading.Tasks.TaskFactory<object>
	// UnityEngine.EventSystems.ExecuteEvents.EventFunction<object>
	// UnityEngine.Events.InvokableCall<UnityEngine.Vector2>
	// UnityEngine.Events.InvokableCall<float>
	// UnityEngine.Events.InvokableCall<object>
	// UnityEngine.Events.UnityAction<UnityEngine.Vector2>
	// UnityEngine.Events.UnityAction<float>
	// UnityEngine.Events.UnityAction<object>
	// UnityEngine.Events.UnityEvent<UnityEngine.Vector2>
	// UnityEngine.Events.UnityEvent<float>
	// UnityEngine.Events.UnityEvent<object>
	// UnityEngine.Pool.CollectionPool.<>c<object,object>
	// UnityEngine.Pool.CollectionPool<object,object>
	// }}

	public void RefMethods()
	{
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,HotFix.core.HotFixEntry.<LoadMetadata>d__2>(System.Runtime.CompilerServices.TaskAwaiter<object>&,HotFix.core.HotFixEntry.<LoadMetadata>d__2&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<HotFix.core.HotFixEntry.<LoadMetadata>d__2>(HotFix.core.HotFixEntry.<LoadMetadata>d__2&)
		// object System.Activator.CreateInstance<object>()
		// object& System.Runtime.CompilerServices.Unsafe.As<object,object>(object&)
		// System.Void* System.Runtime.CompilerServices.Unsafe.AsPointer<object>(object&)
		// object UnityEngine.Component.GetComponent<object>()
		// bool UnityEngine.EventSystems.ExecuteEvents.Execute<object>(UnityEngine.GameObject,UnityEngine.EventSystems.BaseEventData,UnityEngine.EventSystems.ExecuteEvents.EventFunction<object>)
		// System.Void UnityEngine.EventSystems.ExecuteEvents.GetEventList<object>(UnityEngine.GameObject,System.Collections.Generic.IList<UnityEngine.EventSystems.IEventSystemHandler>)
		// bool UnityEngine.EventSystems.ExecuteEvents.ShouldSendToComponent<object>(UnityEngine.Component)
		// object UnityEngine.GameObject.GetComponent<object>()
		// System.Void UnityEngine.GameObject.GetComponents<object>(System.Collections.Generic.List<object>)
	}
}