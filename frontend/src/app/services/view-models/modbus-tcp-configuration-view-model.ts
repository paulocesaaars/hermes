export interface ModbusTcpConfigurationViewModel {
  ip: string;
  port: number;
  scan: number;
  numberOfCoils: number;
  numberOfDiscrete: number;
  numberOfHoldingRegisters: number;
  numberOfInputRegeisters: number;
  maxNumberOfReadAttempts: number;
}
