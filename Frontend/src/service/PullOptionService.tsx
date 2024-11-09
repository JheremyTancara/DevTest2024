import axios from "axios";
import { PollOption, RegisterPollOptionsDTO } from "./types/PollOptionTypes";

class PullOptionsService {
  private baseUrl: string;

  constructor(baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  async getProducts(): Promise<PollOption[]> {
    try {
      const response = await axios.get(`${this.baseUrl}/polloptions`);
      return response.data as PollOption[];
    } catch (error) {
      console.error("Error fetching pulloptions:", error);
      throw error;
    }
  }

  async createProduct(polloption: RegisterPollOptionsDTO): Promise<PollOption> {
    try {
      const response = await axios.post(`${this.baseUrl}/polloptions`, polloption);
      return response.data as PollOption;
    } catch (error) {
      console.error("Error creating polloption:", error);
      throw error;
    }
  }
}

export default PullOptionsService;
