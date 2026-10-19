import { useEffect, useState } from "react";
import { getVideos } from "../api/videos";

function VideoList({ refreshKey }) {
  const [videos, setVideos] = useState([]);
  const [selected, setSelected] = useState(null);
  const [quality, setQuality] = useState("720p");

  useEffect(() => {
    getVideos().then(setVideos);
  }, [refreshKey]);

  function selectVideo(video) {
    setSelected(video);
    if (!video.qualities.includes(quality)) {
      setQuality(video.qualities[0]);
    }
  }

  return (
    <div className="card">
      <h2>Lectures</h2>

      {selected && (
        <div className="player">
          <h3>{selected.title}</h3>
          <p>{selected.description}</p>

          <select value={quality} onChange={(e) => setQuality(e.target.value)}>
            {selected.qualities.map((q) => (
              <option key={q} value={q}>
                {q}
              </option>
            ))}
          </select>

          <video
            key={`${selected.id}-${quality}`}
            controls
            autoPlay
            src={`/api/videos/${selected.id}/stream/${quality}`}
          />
        </div>
      )}

      {videos.length === 0 && <p>No videos yet.</p>}

      <ul className="video-list">
        {videos.map((v) => (
          <li
            key={v.id}
            className={selected?.id === v.id ? "active" : ""}
            onClick={() => selectVideo(v)}
          >
            <strong>{v.title}</strong>
            <span>{new Date(v.createdAt).toLocaleDateString()}</span>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default VideoList;