CREATE TABLE IF NOT EXISTS schema_migrations(
    id INT SERIAL PRIMARY KEY,
    filename TEXT NOT NULL,
    applied_at TIMESTAMP NOT NULL DEFAULT now()
);