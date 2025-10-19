CREATE TABLE IF NOT EXISTS folders(
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(255) NOT NULL,
    description TEXT,
    user_id NOT NULL UUID references users(id)
);