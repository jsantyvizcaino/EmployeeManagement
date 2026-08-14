export interface ApiResponse<T> {
  succeed: boolean
  message?: string
  messageId?: string
  messageType?: number
  result: T
}

export interface ApiListResponse<T> extends ApiResponse<T[]> {
  records: number
}

export interface LoginResponse {
  accessToken: string
  tokenType: string
  userId: number
  employeeId: number
  userName: string
  fullName: string
}

export interface Area {
  id: number
  name: string
  description?: string
}

export interface Position {
  id: number
  name: string
  description?: string
}

export interface Employee {
  id: number
  documentNumber: string
  firstName: string
  lastName: string
  birthDate: string
  age: number
  areaId: number
  areaName: string
  positionId: number
  positionName: string
  monthlyAmount: number
}

export interface CreateEmployeeRequest {
  userName: string
  password: string
  documentNumber: string
  firstName: string
  lastName: string
  birthDate: string
  areaId: number
  positionId: number
  monthlyAmount: number
}
