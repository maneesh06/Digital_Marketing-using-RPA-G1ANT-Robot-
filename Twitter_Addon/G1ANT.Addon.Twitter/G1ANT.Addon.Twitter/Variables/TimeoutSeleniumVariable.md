# timeoutselenium

## Syntax

```G1ANT
♥timeoutselenium = ⟦timespan⟧
```

## Description

Determines the timeout value (in ms) for several `selenium.` commands; the default value is 10000 (10 seconds).

## Example

```G1ANT
♥timeoutselenium = 50
selenium.open chrome url g1ant.com
```

The 50ms timeout in the script above is not enough to complete the process, so an error message appears.