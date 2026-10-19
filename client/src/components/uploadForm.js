import { useState } from "react";
import { uploadVideo, convertVideo } from "../api/videos";

function UploadForm({ onUploaded }) {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [file, setFile] = useState(null);
  const [message, setMessage] = useState({ text: "", type: "" });
  const [busy, setBusy] = useState(false);

  async function handleSubmit(e) {
    e.preventDefault();
    setBusy(true);

    try {
      setMessage({ text: "Uploading video...", type: "success" });
      const video = await uploadVideo(file, title, description);

      setMessage({
        text: "Converting to 480p, 720p and 1080p. This can take a few minutes...",
        type: "success",
      });
      const result = await convertVideo(video.id);

      if (result.status === "Ready") {
        setMessage({ text: "Video is ready.", type: "success" });
        setTitle("");
        setDescription("");
        setFile(null);
        e.target.reset();
        onUploaded();
      } else {
        setMessage({ text: "Conversion failed.", type: "error" });
      }
    } catch (err) {
      setMessage({ text: err.message, type: "error" });
    } finally {
      setBusy(false);
    }
  }

  return (
    <div className="card">
      <h2>Upload a lecture</h2>

      {message.text && (
        <p className={`message ${message.type}`}>{message.text}</p>
      )}

      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label>Title</label>
          <input
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            required
          />
        </div>

        <div className="form-group">
          <label>Description</label>
          <input
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />
        </div>

        <div className="form-group">
          <label>Video file</label>
          <input
            type="file"
            accept="video/*"
            onChange={(e) => setFile(e.target.files[0])}
            required
          />
        </div>

        <button type="submit" className="btn" disabled={busy}>
          {busy ? "Please wait..." : "Upload"}
        </button>
      </form>
    </div>
  );
}

export default UploadForm;