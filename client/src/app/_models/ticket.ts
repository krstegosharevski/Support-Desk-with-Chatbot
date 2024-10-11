import { User } from "./user"

export interface Ticket {
  ticketId: number
  title: string
  message: string
  response: string
  picturePath: string
  status: number
  generatedTime: Date
  finishTime: Date
  createdBy: {id:number}
  createdById: number
  assignedToId: number
  assignedTo: any
  //comments: any
}