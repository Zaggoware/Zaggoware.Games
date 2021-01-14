module.exports = {

    "ignoreFiles": ["./src/scss/_shame.scss", "./src/scss/vendors/**/*.scss", , "**/*.css"],
    "rules": {
        "at-rule-name-case": "lower",
        "at-rule-name-space-after": "always",
        "at-rule-no-vendor-prefix": true,
        "at-rule-semicolon-newline-after": "always",
        "at-rule-semicolon-space-before": "never",

        "block-opening-brace-newline-before": "never-single-line",
        "block-opening-brace-newline-after": "always",
        "block-closing-brace-empty-line-before": "never",
        "block-closing-brace-newline-after": "always",
        "block-closing-brace-newline-before": "always",
        "block-no-empty": true,

        "color-hex-case": "lower",
        "color-hex-length": "long",
        "color-named": "never",
        "color-no-invalid-hex": true,

        "comment-empty-line-before": "always",
        "comment-whitespace-inside": "always",

        "declaration-bang-space-after": "never",
        "declaration-bang-space-before": "always",
        "declaration-block-no-duplicate-properties": true,
        "declaration-block-no-redundant-longhand-properties": true,
        "declaration-block-no-shorthand-property-overrides": true,
        "declaration-block-semicolon-newline-after": "always",
        "declaration-block-semicolon-newline-before": "never-multi-line",
        "declaration-block-semicolon-space-before": "never",
        "declaration-block-single-line-max-declarations": 1,
        "declaration-block-trailing-semicolon": "always",
        "declaration-colon-space-after": "always",
        "declaration-colon-space-before": "never",
        "declaration-empty-line-before": "never",
        "declaration-no-important": true,

        "font-family-no-duplicate-names": true,
        "font-weight-notation": "numeric",

        "function-calc-no-unspaced-operator": true,
        "function-comma-newline-after": "never-multi-line",
        "function-comma-newline-before": "never-multi-line",
        "function-comma-space-after": "always",
        "function-comma-space-before": "never",
        "function-max-empty-lines": 0,
        "function-name-case": "lower",
        "function-parentheses-newline-inside": "always-multi-line",
        "function-parentheses-space-inside": "never",
        "function-url-quotes": "always",
        "function-whitespace-after": "always",

        "indentation": 4,
        "keyframe-declaration-no-important": true,
        "length-zero-no-unit": true,

        "max-empty-lines": 1,

        "media-feature-colon-space-after": "always",
        "media-feature-colon-space-before": "never",
        "media-feature-name-case": "lower",

        "media-feature-parentheses-space-inside": "never",
        "media-feature-range-operator-space-after": "always",
        "media-feature-range-operator-space-before": "always",
        "media-query-list-comma-newline-after": "always-multi-line",
        "media-query-list-comma-newline-before": "never-multi-line",
        "media-query-list-comma-space-after": "always-single-line",
        "media-query-list-comma-space-before": "never",

        "no-descending-specificity": true,
        "no-duplicate-selectors": true,
        "no-eol-whitespace": true,
        "no-extra-semicolons": true,
        "no-invalid-double-slash-comments": true,
        "no-unknown-animations": true,

        "number-max-precision": 3,
        "number-no-trailing-zeros": true,

        "property-case": "lower",
        "property-no-unknown": true,
        "property-no-vendor-prefix": true,

        "rule-empty-line-before": [
            "always",
            {"except": ["first-nested"]}
        ],

        "selector-attribute-brackets-space-inside": "never",
        "selector-attribute-operator-space-after": "never",
        "selector-attribute-operator-space-before": "never",
        "selector-attribute-quotes": "always",
        "selector-class-pattern": "^(?=[a-z])([a-z0-9]-?)+(?<!-)$",
        "selector-combinator-space-after": "always",
        "selector-combinator-space-before": "always",
        "selector-descendant-combinator-no-non-space": true,
        "selector-list-comma-newline-after": "always-multi-line",
        "selector-list-comma-newline-before": "never-multi-line",

        "selector-list-comma-space-after": "always-single-line",
        "selector-list-comma-space-before": "never",
        "selector-max-empty-lines": 0,
        "selector-max-id": 0,
        "selector-nested-pattern": "^(&(_{2}|-{2}|:{1,2})(?=[a-z])([a-z0-9]-?)+(?<!-))(.{0,2}\\s*(&(_{2}|-{2}|:{1,2})(?=[a-z])([a-z0-9]-?)+(?<!-)))*?(\\([a-z+1-9]*\\))?$|^(?:path|svg|svg.[a-z]+)$",
        "selector-no-qualifying-type": true,
        "selector-no-vendor-prefix": true,
        "selector-pseudo-class-case": "lower",
        "selector-pseudo-class-no-unknown": true,
        "selector-pseudo-class-parentheses-space-inside": "never",
        "selector-pseudo-element-case": "lower",
        "selector-pseudo-element-colon-notation": "double",
        "selector-pseudo-element-no-unknown": true,
        "selector-type-case": "lower",
        "selector-type-no-unknown": true,

        "shorthand-property-no-redundant-values": true,
        "string-no-newline": true,
        "string-quotes": "single",
        "time-min-milliseconds": 100,

        "unit-case": "lower",
        "value-no-vendor-prefix": true
    }
};
