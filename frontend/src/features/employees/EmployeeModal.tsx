import { useState, type ChangeEvent, type FormEvent, type MouseEvent } from 'react'
import { createEmployee, getErrorMessage } from '../../api/api'
import type { Area, CreateEmployeeRequest, Position } from '../../api/types'

interface EmployeeModalProps {
  token: string
  areas: Area[]
  positions: Position[]
  onClose: () => void
  onCreated: () => void
}

function EmployeeModal({
  token,
  areas,
  positions,
  onClose,
  onCreated,
}: EmployeeModalProps) {
  const [form, setForm] = useState<CreateEmployeeRequest>({
    userName: '',
    password: '',
    documentNumber: '',
    firstName: '',
    lastName: '',
    birthDate: '',
    areaId: areas[0]?.id ?? 0,
    positionId: positions[0]?.id ?? 0,
    monthlyAmount: 0,
  })
  const [error, setError] = useState('')
  const [saving, setSaving] = useState(false)

  function handleChange(event: ChangeEvent<HTMLInputElement | HTMLSelectElement>) {
    const { name, value } = event.target
    const numericFields = ['areaId', 'positionId', 'monthlyAmount']

    setForm((current) => ({
      ...current,
      [name]: numericFields.includes(name) ? Number(value) : value,
    }))
  }

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    setError('')
    setSaving(true)

    try {
      await createEmployee(token, form)
      onCreated()
    } catch (requestError) {
      setError(getErrorMessage(requestError))
    } finally {
      setSaving(false)
    }
  }

  function handleBackdropClick(event: MouseEvent<HTMLDivElement>) {
    if (event.target === event.currentTarget) onClose()
  }

  return (
    <div className="modal-backdrop" onMouseDown={handleBackdropClick}>
      <section className="modal-card" role="dialog" aria-modal="true" aria-labelledby="modal-title">
        <div className="modal-header">
          <div>
            <p className="eyebrow">Nuevo registro</p>
            <h2 id="modal-title">Agregar empleado</h2>
          </div>
          <button type="button" className="icon-button" onClick={onClose} aria-label="Cerrar">
            ×
          </button>
        </div>

        <form onSubmit={handleSubmit}>
          <div className="form-grid">
            <label>
              Usuario
              <input name="userName" value={form.userName} onChange={handleChange} required maxLength={100} />
            </label>
            <label>
              Contraseña
              <input name="password" type="password" value={form.password} onChange={handleChange} required minLength={8} maxLength={50} />
            </label>
            <label>
              Documento
              <input name="documentNumber" value={form.documentNumber} onChange={handleChange} required maxLength={20} />
            </label>
            <label>
              Fecha de nacimiento
              <input name="birthDate" type="date" value={form.birthDate} onChange={handleChange} required />
            </label>
            <label>
              Nombres
              <input name="firstName" value={form.firstName} onChange={handleChange} required maxLength={100} />
            </label>
            <label>
              Apellidos
              <input name="lastName" value={form.lastName} onChange={handleChange} required maxLength={100} />
            </label>
            <label>
              Área
              <select name="areaId" value={form.areaId} onChange={handleChange} required>
                {areas.map((area) => (
                  <option key={area.id} value={area.id}>{area.name}</option>
                ))}
              </select>
            </label>
            <label>
              Cargo
              <select name="positionId" value={form.positionId} onChange={handleChange} required>
                {positions.map((position) => (
                  <option key={position.id} value={position.id}>{position.name}</option>
                ))}
              </select>
            </label>
            <label className="full-width">
              Salario mensual
              <input name="monthlyAmount" type="number" value={form.monthlyAmount || ''} onChange={handleChange} required min="0.01" step="0.01" />
            </label>
          </div>

          {error && <p className="alert alert-error">{error}</p>}

          <div className="modal-actions">
            <button type="button" className="button button-secondary" onClick={onClose}>
              Cancelar
            </button>
            <button type="submit" className="button button-primary" disabled={saving}>
              {saving ? 'Guardando...' : 'Guardar empleado'}
            </button>
          </div>
        </form>
      </section>
    </div>
  )
}

export default EmployeeModal
