import { useEffect, useState } from "react";
import { getMe, logout } from "./api/auth";
import LoginForm from "./components/LoginForm";
import UploadForm from "./components/UploadForm";
import VideoList from "./components/VideoList";

function App() {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [refreshKey, setRefreshKey] = useState(0);

  useEffect(() => {
    getMe().then((u) => {
      setUser(u);
      setLoading(false);
    });
  }, []);

  async function handleLogout() {
    await logout();
    setUser(null);
  }

  if (loading) return <p className="container">Loading...</p>;

  if (!user) return <LoginForm onLogin={setUser} />;

  return (
    <>
      <header className="navbar">
        <h1>CloudLecture</h1>
        <div className="navbar-user">
          <span>{user.email}</span>
          <span className="badge">{user.role}</span>
          <button className="btn btn-outline" onClick={handleLogout}>
            Logout
          </button>
        </div>
      </header>

   <main className="container">
  {user.role === "Professor" && (
    <UploadForm onUploaded={() => setRefreshKey((k) => k + 1)} />
  )}
  <VideoList refreshKey={refreshKey} />
</main>
    </>
  );
}

export default App;