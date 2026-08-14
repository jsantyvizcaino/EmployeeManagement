import { useState, type FormEvent } from 'react'
import { getErrorMessage, login } from '../../api/api'

interface LoginProps {
  onLogin: (token: string, userName: string) => void
}

function Login({ onLogin }: LoginProps) {
  const [userName, setUserName] = useState('admin')
  const [password, setPassword] = useState('ProCredit2026*')
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    setError('')
    setLoading(true)

    try {
      const response = await login(userName, password)
      onLogin(response.result.accessToken, response.result.userName)
    } catch (requestError) {
      setError(getErrorMessage(requestError))
    } finally {
      setLoading(false)
    }
  }

  return (
    <main className="login-page">
      <section className="login-card">
        <div className="brand-mark">EM</div>
        <p className="eyebrow">Employee Management</p>
        <h1>Iniciar sesión</h1>
        <p className="muted">Ingrese sus credenciales para continuar.</p>

        <form onSubmit={handleSubmit} className="form-stack">
          <label>
            Usuario
            <input
              value={userName}
              onChange={(event) => setUserName(event.target.value)}
              autoComplete="username"
              required
            />
          </label>

          <label>
            Contraseña
            <input
              type="password"
              value={password}
              onChange={(event) => setPassword(event.target.value)}
              autoComplete="current-password"
              required
            />
          </label>

          {error && <p className="alert alert-error">{error}</p>}

          <button type="submit" className="button button-primary" disabled={loading}>
            {loading ? 'Ingresando...' : 'Ingresar'}
          </button>
        </form>

        <p className="login-help">Usuario de prueba: admin</p>
      </section>
    </main>
  )
}

export default Login
