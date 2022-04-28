## Resources

https://vuejs.org/guide/scaling-up/testing.html

This folder will cover `unit test` and `component test`
```
- unit test
- component test
```


- e2e test -> [here](../cypress/README.md)

## Setup

```js
test: {
    globals: true, // instead import { describe, expect, it, test,... }
    environment: 'jsdom',
  },
```

- https://vuejs.org/guide/scaling-up/testing.html#recipes

#### examples
- https://github.com/vitest-dev/vitest/tree/main/examples/vitesse

## Using

- https://testing-library.com/docs/queries/about
- https://testing-library.com/docs/dom-testing-library/api-events
- https://vitest.dev/api/
- https://mswjs.io/docs/getting-started/mocks/rest-api


## References

- https://kentcdodds.com/blog/stop-mocking-fetch
- https://mswjs.io/docs/recipes