import { describe, it, expect } from 'vitest';

describe('Frontend Test', () => {
  it('плогик', () => {
    expect(2 + 2).toBe(4);
  });

  it('переменн окружение', () => {
    const testVar = "checked";
    expect(testVar).toBeDefined();
  });
});