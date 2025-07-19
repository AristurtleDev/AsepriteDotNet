## Documentation Style Guidelines:

1. Follow .NET and MSDN tone and style conventions
2. Only use `<value>` tags when they provide non-redundant information beyond the `<summary>`
3. Provide only useful, non-obvious information - avoid stating the obvious (e.g., don't say "Gets or sets the name" for a property called Name)
4. Do not include `<example>` sections
5. Document formulas and mathematical relationships where relevant for understanding
6. Only add `<remarks>` sections when they provide meaningful additional context
7. Avoid generalized statements - be specific and practical
8. Only document public API members unless private/internal implementations are complex enough to justify documentation

## Documentation Content Focus:

- Explain the purpose and behavior clearly
- Document parameter ranges, units, and constraints where applicable
- Explain relationships between components and their interactions
- Provide practical usage context and applications
- Document performance considerations when relevant
- Explain mathematical formulas and algorithms when they aid understanding
- Clarify error conditions and exceptions
- Note thread safety considerations if applicable

## Technical Details to Include:

- Parameter validation and constraints
- Return value meanings and ranges
- Side effects and state changes
- Performance characteristics
- Integration with other system components
- Mathematical foundations where applicable
