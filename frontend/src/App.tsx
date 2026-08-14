import { useState } from 'react'
import Login from './features/auth/Login'
import Employees from './features/employees/Employees'

const TOKEN_KEY = 'employee_management_token'
const USER_KEY = 'employee_management_user'

function App() {
  const [token, setToken] = useState(() => localStorage.getItem(TOKEN_KEY) ?? '')
  const [userName, setUserName] = useState(
    () => localStorage.getItem(USER_KEY) ?? '',
  )

  function handleLogin(accessToken: string, authenticatedUser: string) {
    localStorage.setItem(TOKEN_KEY, accessToken)
    localStorage.setItem(USER_KEY, authenticatedUser)
    setToken(accessToken)
    setUserName(authenticatedUser)
  }

  function handleLogout() {
    localStorage.removeItem(TOKEN_KEY)
    localStorage.removeItem(USER_KEY)
    setToken('')
    setUserName('')
  }

  if (!token) {
    return <Login onLogin={handleLogin} />
  }

  return <Employees token={token} userName={userName} onLogout={handleLogout} />
}

export default App
