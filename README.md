# Algebra Calculator API

This API is a work in progress and aims to provide seamless algebraic operations, including basic arithmetic, fractions, exponentials, and polynomial manipulation.

## Features

- **Number Storage**: Store numerical values, fractional values, and expressions involving other numbers.
- **Basic Arithmetic**: Perform addition, subtraction, multiplication, and division on algebraic entities.
- **Polynomial Factoring**: Currently supports factoring out common factors from polynomial expressions.

## Getting Started

To start using the Algebra Calculator API in your C# project, follow these steps:

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/your-username/algebra-calculator-api.git
   ```

2. **Reference the API**:
   - Open your C# project in your preferred IDE.
   - Add a reference to the `Algebra Calculator.dll` file in your project.

4. **Perform Algebraic Operations**:
   ```csharp
   // Store values
   Value value1 = new RationalValue(5);    // Store a numeric value
   Value value2 = new RationalValue(2, 3); // Store a fractional value (2/3)

   // Store an expression
   Expression expression1 = new(new List<Number>() {
       new(3, new() { new Letter('x') }),
       new(4)
   });

   // Perform arithmetic operations
   Number resultAddition = new Number(2) + new Number(4);    // Addition
   Number resultSubtraction = new Number(5) - new Number(3); // Subtraction

   // Perform polynomial factoring
   Expression polynomial = new(new List<Number>() {
       new(3, new() { new Letter('x', new(2))}),
       new(6, new() { new Letter('x')})
   });
   
   Number? factoredPolynomial = Utilities.Polynomials.Factorise(polynomial);
   ```
   
## License

This project is licensed under the [MIT License](LICENSE).
