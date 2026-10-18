import { useState } from "react";
import { login, register } from "../api/auth";

function LoginForm({ onLogin }) {
  const [isRegister, setIsRegister] = useState(false);
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [role, setRole] = useState("Student");
  const [message, setMessage] = useState({ text: "", type: "" });

  async function handleSubmit(e) {
    e.preventDefault();
    setMessage({ text: "", type: "" });

    try {
      if (isRegister) {
        await register(email, password, role);
        setMessage({ text: "Account created. You can log in now.", type: "success" });
        setIsRegister(false);
      } else {
        const user = await login(email, password);
        onLogin(user);
      }
    } catch (err) {
      setMessage({ text: err.message, type: "error" });
    }
  }

  return (
    <div className="auth-page">
      <div className="auth-card">
        <h1>CloudLecture</h1>
        <h2>{isRegister ? "Create an account" : "Sign in to continue"}</h2>

        {message.text && (
          <p className={`message ${message.type}`}>{message.text}</p>
        )}

        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label>Email</label>
            <input
              type="email"
              placeholder="you@example.com"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>

          <div className="form-group">
            <label>Password</label>
            <input
              type="password"
              placeholder="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>

          {isRegister && (
            <div className="form-group">
              <label>I am a</label>
              <select value={role} onChange={(e) => setRole(e.target.value)}>
                <option value="Student">Student</option>
                <option value="Professor">Professor</option>
              </select>
            </div>
          )}

          <button type="submit" className="btn">
            {isRegister ? "Register" : "Login"}
          </button>
        </form>

        <button
          type="button"
          className="btn-link"
          onClick={() => {
            setIsRegister(!isRegister);
            setMessage({ text: "", type: "" });
          }}
        >
          {isRegister ? "I already have an account" : "Create an account"}
        </button>
      </div>
    </div>
  );
}

export default LoginForm;