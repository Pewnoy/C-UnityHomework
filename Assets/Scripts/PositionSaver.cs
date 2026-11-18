using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DefaultNamespace
{
	public class PositionSaver : MonoBehaviour
	{
        [Serializable]
        public struct Data
		{
			public Vector3 Position;
			public float Time;
		}

        [ReadOnly]
        [Tooltip("Для заполнения поля воспользуйтесь контекстным меню и командой Create File")]
        private TextAsset _json;

        [field: SerializeField]
        [field: HideInInspector]
        public List<Data> Records { get; private set; }

        private void Awake()
		{
			//todo comment: Что будет, если в теле этого условия не сделать выход из метода?
			//если убрать return метод продолжится. Unity попытается десериализовать данные из json.text, хотя при этом, он равен null
			if (_json == null)
			{
				gameObject.SetActive(false);
				Debug.LogError("Please, create TextAsset and add in field _json");
				return;
			}
			
			JsonUtility.FromJsonOverwrite(_json.text, this);
            //todo comment: Для чего нужна эта проверка (что она позволяет избежать)?
            //она проверяет были ли создана коллекция Records после десериализациии. Если коллекция отсутствует в JSON, создаётся новый пустой список, чтобы избежать NullReferenceException при дальнейшей работе с ним
            if (Records == null)
				Records = new List<Data>(10);
		}

		private void OnDrawGizmos()
		{
            //todo comment: Зачем нужны эти проверки (что они позволляют избежать)?
            //Records == null - защищает от обращения к неинициализированному списку; Records.Count == 0 -  защищает от попытки получить элемент 0 у пустого списка
            if (Records == null || Records.Count == 0) return;
			var data = Records;
			var prev = data[0].Position;
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(prev, 0.3f);
            //todo comment: Почему итерация начинается не с нулевого элемента?
            //Потому что первый элемент уже получен ранее в var prev = data[0].Position;. цикл начинает работу со второго элемента, чтобы соединять каждую следующую точку с предыдущей
            for (int i = 1; i < data.Count; i++)
			{
				var curr = data[i].Position;
				Gizmos.DrawWireSphere(curr, 0.3f);
				Gizmos.DrawLine(prev, curr);
				prev = curr;
			}
		}
		
#if UNITY_EDITOR
		[ContextMenu("Create File")]
		private void CreateFile()
		{
            //todo comment: Что происходит в этой строке?
            //cоздаётся файл Path.txt в папке Assets проекта Unity. Метод возвращает объект FileStream, позволяющий работать с созданным файлом
            var stream = File.Create(Path.Combine(Application.dataPath, "Path.txt"));
            //todo comment: Подумайте для чего нужна эта строка? (а потом проверьте догадку, закомментировав) 
            //она закрывает поток файла и освобождает связанные с ним системные ресурсы; eсли убрать эту строку поток останется открытым, пока не будет освобождён другим способом
            stream.Dispose();
			UnityEditor.AssetDatabase.Refresh();
			//В Unity можно искать объекты по их типу, для этого используется префикс "t:"
			//После нахождения, Юнити возвращает массив гуидов (которые в мета-файлах задаются, например)
			var guids = UnityEditor.AssetDatabase.FindAssets("t:TextAsset");
			foreach (var guid in guids)
			{
				//Этой командой можно получить путь к ассету через его гуид
				var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
				//Этой командой можно загрузить сам ассет
				var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                //todo comment: Для чего нужны эти проверки?
                //asset != null - проверяет, удалось ли загрузить объект TextAsset; asset.name == "Path" - проверяет, что найден именно нужный файл Path.txt
                if (asset != null && asset.name == "Path")
				{
					_json = asset;
					UnityEditor.EditorUtility.SetDirty(this);
					UnityEditor.AssetDatabase.SaveAssets();
					UnityEditor.AssetDatabase.Refresh();
                    //todo comment: Почему мы здесь выходим, а не продолжаем итерироваться?
                    //потому что нужный файл уже найден и присвоен полю _json. продолжать поиск остальных файлов нет необходимости
                    return;
				}
			}
		}

		private void OnDestroy()
		{
			//todo logic...
		}
#endif
	}
}