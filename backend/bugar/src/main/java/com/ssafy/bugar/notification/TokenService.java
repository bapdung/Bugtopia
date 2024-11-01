package com.ssafy.bugar.notification;

import java.util.HashSet;
import java.util.Set;
import org.springframework.stereotype.Service;

@Service
public class TokenService {
    private final Set<String> tokens = new HashSet<>();

    public void saveToken(String token) {
        tokens.add(token);
    }

    public Set<String> getAllTokens() {
        return tokens;
    }
}
