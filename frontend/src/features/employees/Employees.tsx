import { useEffect, useState } from 'react'
import { getAreas, getEmployees, getErrorMessage, getPositions } from '../../api/api'
import type { Area, Employee, Position } from '../../api/types'
import EmployeeModal from './EmployeeModal'

interface EmployeesProps {
  token: string
  userName: string
  onLogout: () => void
}

const currencyFormatter = new Intl.NumberFormat('es-EC', {
  style: 'currency',
  currency: 'USD',
})

function Employees({ token, userName, onLogout }: EmployeesProps) {
  const [employees, setEmployees] = useState<Employee[]>([])
  const [areas, setAreas] = useState<Area[]>([])
  const [positions, setPositions] = useState<Position[]>([])
  const [selectedAreaId, setSelectedAreaId] = useState('')
  const [showModal, setShowModal] = useState(false)
  const [refreshKey, setRefreshKey] = useState(0)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    let active = true

    Promise.all([getAreas(token), getPositions(token)])
      .then(([areasResponse, positionsResponse]) => {
        if (!active) return
        setAreas(areasResponse.result)
        setPositions(positionsResponse.result)
      })
      .catch((requestError) => {
        if (active) setError(getErrorMessage(requestError))
      })

    return () => {
      active = false
    }
  }, [token])

  useEffect(() => {
    let active = true
    setLoading(true)
    setError('')

    const areaId = selectedAreaId ? Number(selectedAreaId) : undefined
    getEmployees(token, areaId)
      .then((response) => {
        if (active) setEmployees(response.result)
      })
      .catch((requestError) => {
        if (active) setError(getErrorMessage(requestError))
      })
      .finally(() => {
        if (active) setLoading(false)
      })

    return () => {
      active = false
    }
  }, [token, selectedAreaId, refreshKey])

  function handleEmployeeCreated() {
    setShowModal(false)
    setRefreshKey((current) => current + 1)
  }

  return (
    <main className="dashboard-page">
      <header className="topbar">
        <div className="brand-row">
          <div className="brand-mark brand-mark-small">EM</div>
          <div>
            <strong>Employee Management</strong>
            <span>Administración de personal</span>
          </div>
        </div>
        <div className="user-actions">
          <span>Hola, {userName}</span>
          <button type="button" className="button button-secondary" onClick={onLogout}>
            Cerrar sesión
          </button>
        </div>
      </header>

      <section className="content">
        <div className="page-heading">
          <div>
            <p className="eyebrow">Personal</p>
            <h1>Empleados</h1>
            <p className="muted">Consulte y registre empleados de la organización.</p>
          </div>
          <button
            type="button"
            className="button button-primary"
            onClick={() => setShowModal(true)}
          >
            + Nuevo empleado
          </button>
        </div>

        <div className="summary-grid">
          <article className="summary-card">
            <span>Empleados visibles</span>
            <strong>{employees.length}</strong>
          </article>
          <article className="summary-card">
            <span>Áreas disponibles</span>
            <strong>{areas.length}</strong>
          </article>
          <article className="summary-card">
            <span>Cargos disponibles</span>
            <strong>{positions.length}</strong>
          </article>
        </div>

        <section className="table-card">
          <div className="table-toolbar">
            <div>
              <h2>Listado de empleados</h2>
              <p className="muted">La edad se calcula desde la fecha de nacimiento.</p>
            </div>
            <label className="filter-field">
              Filtrar por área
              <select
                value={selectedAreaId}
                onChange={(event) => setSelectedAreaId(event.target.value)}
              >
                <option value="">Todas las áreas</option>
                {areas.map((area) => (
                  <option key={area.id} value={area.id}>
                    {area.name}
                  </option>
                ))}
              </select>
            </label>
          </div>

          {error && <p className="alert alert-error table-alert">{error}</p>}

          <div className="table-wrapper">
            <table>
              <thead>
                <tr>
                  <th>Documento</th>
                  <th>Empleado</th>
                  <th>Edad</th>
                  <th>Área</th>
                  <th>Cargo</th>
                  <th className="align-right">Salario</th>
                </tr>
              </thead>
              <tbody>
                {loading ? (
                  <tr>
                    <td colSpan={6} className="empty-cell">Cargando empleados...</td>
                  </tr>
                ) : employees.length === 0 ? (
                  <tr>
                    <td colSpan={6} className="empty-cell">No se encontraron empleados.</td>
                  </tr>
                ) : (
                  employees.map((employee) => (
                    <tr key={employee.id}>
                      <td>{employee.documentNumber}</td>
                      <td>
                        <strong>{employee.firstName} {employee.lastName}</strong>
                        <span className="cell-detail">Nacimiento: {employee.birthDate}</span>
                      </td>
                      <td>{employee.age} años</td>
                      <td><span className="badge">{employee.areaName}</span></td>
                      <td>{employee.positionName}</td>
                      <td className="align-right salary">
                        {currencyFormatter.format(employee.monthlyAmount)}
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        </section>
      </section>

      {showModal && (
        <EmployeeModal
          token={token}
          areas={areas}
          positions={positions}
          onClose={() => setShowModal(false)}
          onCreated={handleEmployeeCreated}
        />
      )}
    </main>
  )
}

export default Employees
