---
title: Matrix Extensions
author: michael-hawker
description: UWP Matrix extensions methods
keywords: windows 10, uwp, windows community toolkit, uwp community toolkit, uwp toolkit, Extensions, matrix
dev_langs:
  - csharp
---

# Matrix Extensions
The [Matrix Extensions](https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.ui.extensions.matrixextensions) provide methods to transform a [Matrix](https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Xaml.Media.Matrix) (Rotate, Scale, Translate, etc...).  These are a similar subset of methods originally provided in the [System.Windows.Media.Matrix](https://msdn.microsoft.com/en-us/library/system.windows.media.matrix(v=vs.110).aspx) class.

## Methods

| Methods | Return Type | Description |
| -- | -- | -- |
| HasInverse | bool | Returns true if the matrix has an inverse. |
| Multiply(Matrix) | Matrix | Multiply this matrix to the given matrix and return the result. |
| Rotate(Matrix, double) | Matrix | Applies a rotation of the specified angle about the origin of this Matrix structure and returns the result. |
| RotateAt(Matrix, double, double, double) | Matrix | Rotates this matrix about the specified point and returns the new result. |
| Scale(double, double) | Matrix | Appends the specified scale vector to this Matrix structure and returns the result. |
| ScaleAt(double, double, double, double) | Matrix | Scales this Matrix by the specified amount about the specified point and returns the result. |
| Skew(double, double) | Matrix | Appends a skew of the specified degrees in the x and y dimensions to this Matrix structure and returns the result. |
| Translate(double, double) | Matrix | Translates the matrix by the given amount and returns the result. |

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI |
| NuGet package | [Microsoft.Toolkit.Uwp.UI](https://www.nuget.org/packages/Microsoft.Toolkit.Uwp.UI/) |

## API Source Code

- [MatrixExtensions source code](https://github.com/Microsoft/WindowsCommunityToolkit//blob/master/Microsoft.Toolkit/Extensions/Media/MatrixExtensions.cs)

## Related Topics

- [Windows.UI.Xaml.Media.Matrix](https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Xaml.Media.Matrix)
- [System.Windows.Media.Matrix](https://msdn.microsoft.com/en-us/library/system.windows.media.matrix(v=vs.110).aspx)
