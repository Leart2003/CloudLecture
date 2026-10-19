export async function getVideos() {
  const res = await fetch("/api/videos");
  if (!res.ok) return [];
  return await res.json();
}

export async function uploadVideo(file, title, description) {
  const formData = new FormData();
  formData.append("file", file);
  formData.append("title", title);
  formData.append("description", description);

 
  const res = await fetch("/api/videos/upload", {
    method: "POST",
    body: formData,
  });

  if (!res.ok) throw new Error("Upload failed");
  return await res.json(); 
}

export async function convertVideo(id) {
  const res = await fetch(`/api/videos/${id}/convert`, { method: "POST" });
  if (!res.ok) throw new Error("Conversion failed");
  return await res.json(); 
}