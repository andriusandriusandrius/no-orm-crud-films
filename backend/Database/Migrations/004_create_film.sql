CREATE TABLE IF NOT EXISTS films(
    id UUID NOT NULL DEFAULT gen_random_uuid(),
    name VARCHAR(255) NOT NULL,
    personal_review TEXT,
    rating INT,
    CHECK (rating BETWEEN 1 AND 10),
    folder_id UUID NOT NULL references folders(id)
);