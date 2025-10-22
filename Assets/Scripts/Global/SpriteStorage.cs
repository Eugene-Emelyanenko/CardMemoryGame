using System.Collections;
using System.Collections.Generic;
using Card;
using UnityEngine;
using UnityEngine.Networking;

namespace Global
{
    public class SpriteStorage
    {
        private const string JSON_URL =
            "https://drive.google.com/uc?export=download&id=1LWxIfZfMcw-PMR8TaUjkiZG_glv-TdzL";

        private const int MaxConcurrentDownloads = 4;

        public bool IsLoaded { get; private set; }
        public float Progress { get; private set; }

        private readonly List<CardData> _allCards = new();
        public IReadOnlyList<CardData> AllCards => _allCards;

        public CardData GetBack() => _allCards.Find(c => c.Id == "Back");
        public List<CardData> GetFaces() => _allCards.FindAll(c => c.Id != "Back");

        public void Clear()
        {
            _allCards.Clear();
            IsLoaded = false;
            Progress = 0f;
        }

        public IEnumerator PreloadAll()
        {
            Clear();

            using (UnityWebRequest req = UnityWebRequest.Get(JSON_URL))
            {
                yield return req.SendWebRequest();
                if (req.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"JSON load failed: {req.error}");
                    yield break;
                }

                string json = req.downloadHandler.text.TrimStart('\uFEFF', '\u200B', '\u200E', '\u200F');

                CardJsonRoot root = null;
                try
                {
                    root = JsonUtility.FromJson<CardJsonRoot>(json);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"JSON parse error: {e}");
                    yield break;
                }

                if (root?.images == null || root.images.Length == 0)
                {
                    Debug.LogError("JSON parsed, but 'images' is empty");
                    yield break;
                }

                yield return DownloadSprites(root.images);
            }

            IsLoaded = true;
            Progress = 1f;
        }

        private IEnumerator DownloadSprites(CardJsonEntry[] entries)
        {
            var queue = new Queue<CardJsonEntry>(entries);
            int total = entries.Length;
            int done = 0;

            var workers = new List<IEnumerator>(MaxConcurrentDownloads);
            for (int i = 0; i < MaxConcurrentDownloads; i++)
            {
                workers.Add(Worker());
            }

            var active = new List<IEnumerator>(workers);
            while (active.Count > 0)
            {
                for (int i = active.Count - 1; i >= 0; i--)
                {
                    if (!active[i].MoveNext()) active.RemoveAt(i);
                    else if (active[i].Current != null) yield return active[i].Current;
                }

                Progress = (float)done / total;
                yield return null;
            }

            IEnumerator Worker()
            {
                while (queue.Count > 0)
                {
                    var task = queue.Dequeue();
                    yield return LoadSprite(task.id, task.url);
                    done++;
                }
            }
        }

        private IEnumerator LoadSprite(string id, string url)
        {
            using (UnityWebRequest texReq = UnityWebRequestTexture.GetTexture(url, true))
            {
                yield return texReq.SendWebRequest();
                if (texReq.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Texture load failed: {id} | {texReq.error}");
                    yield break;
                }

                var tex = DownloadHandlerTexture.GetContent(texReq);
                var sprite = Sprite.Create(
                    tex,
                    new Rect(0, 0, tex.width, tex.height),
                    new Vector2(0.5f, 0.5f),
                    100f
                );
                _allCards.Add(new CardData(id, sprite));
            }
        }
    }
}