import type {
  ApiListResponse,
  ApiResponse,
  Area,
  CreateEmployeeRequest,
  Employee,
  LoginResponse,
  Position,
} from './types'

const API_URL = (
  import.meta.env.VITE_API_URL ?? 'https://localhost:7059/api/v1'
).replace(/\/$/, '')

async function request<T>(
  path: string,
  options: RequestInit = {},
  token?: string,
): Promise<T> {
  const headers = new Headers(options.headers)

  if (options.body) {
    headers.set('Content-Type', 'application/json')
  }

  if (token) {
    headers.set('Authorization', `Bearer ${token}`)
  }

  const response = await fetch(`${API_URL}${path}`, {
    ...options,
    headers,
  })
  const body = await response.text()
  const data = body ? (JSON.parse(body) as T & { succeed?: boolean; message?: string }) : null

  if (!response.ok || !data?.succeed) {
    throw new Error(data?.message ?? 'No se pudo completar la solicitud.')
  }

  return data
}

export function getErrorMessage(error: unknown) {
  return error instanceof Error ? error.message : 'Ocurrió un error inesperado.'
}

export function login(userName: string, password: string) {
  return request<ApiResponse<LoginResponse>>('/authentication/login', {
    method: 'POST',
    body: JSON.stringify({ userName, password }),
  })
}

export function getAreas(token: string) {
  return request<ApiListResponse<Area>>('/areas', {}, token)
}

export function getPositions(token: string) {
  return request<ApiListResponse<Position>>('/positions', {}, token)
}

export function getEmployees(token: string, areaId?: number) {
  const query = areaId ? `?areaId=${areaId}` : ''
  return request<ApiListResponse<Employee>>(`/employees${query}`, {}, token)
}

export function createEmployee(
  token: string,
  employee: CreateEmployeeRequest,
) {
  return request<ApiResponse<Employee>>(
    '/employees',
    {
      method: 'POST',
      body: JSON.stringify(employee),
    },
    token,
  )
}
