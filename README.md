# GaussianElimination
C# implementation of Gaussian Elimination for systems of XOR equations.

![Build status](https://switchigan.visualstudio.com/_apis/public/build/definitions/9e65584e-ff3f-4616-b1ab-5227abae1502/7/badge "Build status")

## NuGet

Run this command to install [the package](https://www.nuget.org/packages/Matt.Math.Linear.Solving/):

```Install-Package Matt.Math.Linear.Solving```

## Usage

```csharp
var coefficients = new List<bool[]>();
var solutions = new List<byte[]>();

var solver = new GuassianElimination(coefficients, solutions);

var solution = solver.Solve(); // This modifies the "coefficients" and "solutions" lists
if (solution != null)
    // "solution" is a clone of the first rows of "solutions"
else
    // Not enough systems of equations are represented in "coefficients" and "solutions"
```

For example, these equations:

 * `a XOR b = [0x01, 0x02, 0x03]`
 * `b XOR c = [0x02, 0x03, 0x04]`
 * `a XOR b XOR c = [0x03, 0x04, 0x05]`

...can be solved with this code:

```csharp
var coefficients = new List<bool[]>(
	new [] {true, true, false},
	new [] {false, true, true},
	new [] {true, true, true}
);
var solutions = new List<byte[]>(
	new [] {0x01, 0x02, 0x03},
	new [] {0x02, 0x03, 0x04},
	new [] {0x03, 0x04, 0x05}
);

var solver = new GuassianElimination(coefficients, solutions);

var solution = solver.Solve();
```

Note that solutions are given in [reduced row echelon form](https://en.wikipedia.org/wiki/Row_echelon_form#Reduced_row_echelon_form).

Invoking `.Solve()` mutates the lists passed into the constructor; row operations are performed on them to change them as much as possible into reduced row echelon form.

Therefore, as they become available, new equations can be added to the `coefficients` and `solutions` lists between invocations of `.Solve()`.

## Thread safety

This code is not thread-safe.

## Use cases

This was originally designed for [this rateless forward error-correction project](https://github.com/matthew-a-thomas/cs-distributed-storage). It should be useful for other implementations of fountain codes.
