async function readJson(res) {
  const text = await res.text();
  try {
    return text ? JSON.parse(text) : {};
  } catch {
    return {};
  }
}

function errorMessage(res, data) {
  return data.message || `Request failed (status ${res.status})`;
}

export async function register(email, password, role) {
  const res = await fetch("/api/auth/register", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email, password, role }),
  });
  const data = await readJson(res);
  if (!res.ok) throw new Error(errorMessage(res, data));
  return data;
}

export async function login(email, password) {
  const res = await fetch("/api/auth/login", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email, password }),
  });
  const data = await readJson(res);
  if (!res.ok) throw new Error(errorMessage(res, data));
  return data; // { email, role }
}

export async function logout() {
  await fetch("/api/auth/logout", { method: "POST" });
}

export async function getMe() {
  try {
    const res = await fetch("/api/auth/me");
    if (!res.ok) return null;
    return await res.json(); // { email, role }
  } catch {
    return null;
  }
}