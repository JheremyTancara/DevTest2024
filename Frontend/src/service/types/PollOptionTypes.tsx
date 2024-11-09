export interface PollOption {
  pollOptionID: string;       
  name: string;              
  votes: number;       
  createdAt: string;     
  isDeleted: boolean;        
}

export interface RegisterPollOptionsDTO {
  name: string;
  votes: number;
}